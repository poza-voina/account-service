using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Brokers.CreateDeadLetter;
using AccountService.Api.Features.Brokers.CreateInboxConsumed;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using Hangfire.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace AccountService.Api.ObjectStorage.Events.Consumers;

public abstract class ConsumerBase<T>(
    IRabbitMqConnectionMonitor monitor,
    ConsumerConfiguration consumerConfiguration,
    ILogger<T> logger,
    IServiceProvider serviceProvider)
    : BackgroundService
{
    private const string EventTypePropertyName = "eventType";

    protected abstract Task HandleMessageAsync(string eventType, string message, MessageLogData logData, CancellationToken stoppingToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Handle(stoppingToken);
            }
            catch(Exception ex)
            {
                logger.LogCritical(ex.Message);
            }
        }
    }

    protected async Task Handle(CancellationToken stoppingToken)
    {
        int attempts = 0;
        while (monitor.Connection is null && attempts < 5)
        {
            await Task.Delay(500);
            attempts++;
        }

        if (monitor.Connection is null)
        {
            throw new Exception("Консюмеру не удалось установить соединение с RabbitMQ");
        }

        await using var channel = await monitor.Connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: consumerConfiguration.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (ch, ea) =>
        {

            var logdata = new MessageLogData
            {
                Stopwatch = Stopwatch.StartNew()
            };

            await ProcessMessage(ea, channel, logdata, stoppingToken);

            logdata.Stopwatch.Stop();
            logdata.Latency = logdata.Stopwatch.ElapsedMilliseconds.ToString();
            logger.LogInformation("Message received: {@LogData}", logdata);
        };

        await channel.BasicConsumeAsync(
            queue: consumerConfiguration.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
        );

        while (monitor.Connection?.IsOpen is true)
        {
            await Task.Delay(1000);
        }

        throw new Exception("Connection lost: RabbitMQ broker unreachable");
    }

    private async Task ProcessMessage(BasicDeliverEventArgs ea, IChannel channel, MessageLogData logdata, CancellationToken stoppingToken)
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());

        var messageId = ea.BasicProperties.MessageId;

        logdata.MessageId = messageId?.ToString();

        try
        {
            var eventType = ProcessProperties(message);

            var parsedMesageId = ParseMessageId(message, messageId);

            await CheckMessageIdAsync(parsedMesageId, message);

            await HandleMessageAsync(eventType, message, logdata, stoppingToken);

            var command = new CreateInboxConsumedCommand
            {
                MessageId = parsedMesageId,
                Handler = typeof(T).Name
            };

            await TrySendToMediatorAsync(command);

            // ReSharper disable once AccessToDisposedClosure Это backgroundService
            await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        }
        catch (ConsumerHandleMessageException exception)
        {
            var command = new CreateDeadLetterCommand
            {
                ExceptionMessage = exception.Message,
                EventType = exception.EventType,
                StackTrace = exception.StackTrace
            };

            await TrySendToMediatorAsync(command);

            // ReSharper disable once AccessToDisposedClosure Это backgroundService
            await channel.BasicNackAsync(ea.DeliveryTag, false, false, stoppingToken);

            logger.LogCritical("Не удалось обработать сообщение с messageId {MessageId}", logdata.MessageId);
        }
        catch
        {
            // ReSharper disable once AccessToDisposedClosure Это backgroundService
            await channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);

            logger.LogCritical("Не удалось обработать сообщение с messageId {MessageId}", logdata.MessageId);
        }
    }

    private static Guid ParseMessageId(string message, string? messageId)
    {
        if (messageId is null)
        {
            throw new ConsumerHandleMessageException(message, "MessageId не найдено");
        }

        if (!Guid.TryParse(messageId, out var result))
        {
            throw new ConsumerHandleMessageException(message, "MessageId не удалось преобразовать в GUID");
        }

        return result;
    }

    private async Task CheckMessageIdAsync(Guid messageId, string message)
    {
        using var scope = serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<InboxConsumed>>();

        if (await repository.GetAll().AnyAsync(x => x.MessageId == messageId))
        {
            throw new ConsumerHandleMessageException(message, "Сообщение уже было принято");
        }
    }

    private static string ProcessProperties(string message)
    {
        using var doc = JsonDocument.Parse(message);
        var root = doc.RootElement;

        if (!root.TryGetProperty(EventTypePropertyName, out var typeProperty))
        {
            throw new ConsumerHandleMessageException(message, $"{EventTypePropertyName}");
        }

        var eventType = typeProperty.GetString() ?? throw new ConsumerHandleMessageException(message, $"{EventTypePropertyName} не найдено");

        return eventType;
    }

    protected async Task<bool> TrySendToMediatorAsync<T>(T command)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(command!);
        }
        catch
        {
            return false;
        }

        return true;
    }
}
