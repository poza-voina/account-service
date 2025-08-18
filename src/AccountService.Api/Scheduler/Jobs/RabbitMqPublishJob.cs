using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.Scheduler.Interfaces;
using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AccountService.Api.Scheduler.Jobs;

public class RabbitMqPublishJob(IRabbitMqPublisher rabbitMqPublisher, IUnitOfWork unitOfWork, ILogger<RabbitMqPublishJob> logger) : IJob
{
    public async Task Execute()
    {
        var repository = unitOfWork.GetRepository<IRepository<OutboxMessage>>();

        var batch = new List<OutboxMessage>();
        const int batchSize = 100;

        var data = repository.GetAll()
                .Where(x => x.Status == OutboxStatus.Pending || x.Status == OutboxStatus.Failed)
                .AsAsyncEnumerable();

        await foreach (var item in data)
        {
            batch.Add(item);

            if (batch.Count < batchSize) continue;
            await ProcessBatchAsync(batch, repository);
            batch.Clear();
        }

        if (batch.Count > 0)
        {
            await ProcessBatchAsync(batch, repository);
        }
    }

    private async Task ProcessBatchAsync(List<OutboxMessage> batch, IRepository<OutboxMessage> repository)
    {
        await unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, CancellationToken.None);

        try
        {
            await PublishBatchAsync(batch);

            await repository.UpdateRangeAsync(batch);

            await unitOfWork.CommitAsync(CancellationToken.None);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(CancellationToken.None);
        }
    }

    private async Task PublishBatchAsync(List<OutboxMessage> batch)
    {
        var stopwatch = Stopwatch.StartNew();

        foreach (var item in batch)
        {
            try
            {
                await rabbitMqPublisher.PublishAsync(item.Id, item.EventType, item.EventPayload);

                logger.LogInformation(
                    "Message published: EventId={EventId}, Type={Type}, CorrelationId={CorrelationId}, Retry={Retry}, Latency={Latency}ms",
                    item.Id, item.EventType, item.CorrelationId, item.RetryCount, stopwatch.ElapsedMilliseconds);

                item.ProcessedAt = DateTime.UtcNow;
                item.RetryCount++;
                item.Status = OutboxStatus.Sent;
            }
            catch (Exception exception)
            {
                logger.LogError(
                    "Failed to publish message: EventId={EventId}, Type={Type}, CorrelationId={CorrelationId}, Retry={Retry}, {exception}",
                    item.Id, item.EventType, item.CorrelationId, item.RetryCount, exception.Message);

                item.ProcessedAt = DateTime.UtcNow;
                item.RetryCount++;
                item.Status = OutboxStatus.Failed;
            }
        }
    }
}
