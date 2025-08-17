using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.Scheduler.Interfaces;
using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Api.Scheduler.Jobs;

public class RabbitMqPublishJob(IRabbitMqService rabbitMqService, IUnitOfWork unitOfWork) : IJob
{
    public async Task Execute()
    {
        var repository = unitOfWork.GetRepository<IRepository<OutboxMessage>>();

        List<OutboxMessage> batch = new List<OutboxMessage>();
        var batchSize = 100;

        var data = repository.GetAll()
                .Where(x => x.Status == OutboxStatus.Pending || x.Status == OutboxStatus.Failed)
                .AsAsyncEnumerable();

        await foreach (var item in data)
        {
            batch.Add(item);

            if (batch.Count >= batchSize)
            {
                await ProcessBatchAsync(batch, repository);
                batch.Clear();
            }
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
        foreach (var item in batch)
        {
            try
            {
                await rabbitMqService.PublishAsync(item.EventType, item.EventPayload);
                item.Status = OutboxStatus.Sent;
            }
            catch (Exception)
            {
                item.Status = OutboxStatus.Failed;
            }
        }
    }
}
