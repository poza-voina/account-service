using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Brokers.CreateDeadLetter;
using AccountService.Api.Features.Brokers.CreateInboxConsumed;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
                logger.LogCritical("{message}", ex.Message);
            }
        }
    }

    protected async Task Handle(CancellationToken stoppingToken)
    {
        var attempts = 0;
        while (monitor.Connection is null && attempts < 5)
        {
            await Task.Delay(500, stoppingToken);
            attempts++;
        }

        if (monitor.Connection is null)
        {
            throw new Exception("Consumer не удалось установить соединение с RabbitMQ");
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
        consumer.ReceivedAsync += async (_, ea) =>
        {

            var logData = new MessageLogData
            {
                Stopwatch = Stopwatch.StartNew()
            };

            // ReSharper disable once AccessToDisposedClosure работает
            await ProcessMessage(ea, channel, logData, stoppingToken);

            logData.Stopwatch.Stop();
            logData.Latency = logData.Stopwatch.ElapsedMilliseconds.ToString();
            logger.LogInformation("Message received: {@LogData}", logData);
        };

        await channel.BasicConsumeAsync(
            queue: consumerConfiguration.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
        );

        while (monitor.Connection?.IsOpen is true)
        {
            await Task.Delay(1000, stoppingToken);
        }

        throw new Exception("Connection lost: RabbitMQ broker unreachable");
    }

    private async Task ProcessMessage(BasicDeliverEventArgs ea, IChannel channel, MessageLogData logData, CancellationToken stoppingToken)
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());

        var messageId = ea.BasicProperties.MessageId;

        logData.MessageId = messageId;

        try
        {
            var eventType = ProcessProperties(message);

            var parsedMessageId = ParseMessageId(message, messageId);

            await CheckMessageIdAsync(parsedMessageId, message);

            await HandleMessageAsync(eventType, message, logData, stoppingToken);

            var command = new CreateInboxConsumedCommand
            {
                MessageId = parsedMessageId,
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

            logger.LogCritical("Не удалось обработать сообщение с messageId {MessageId}", logData.MessageId);
        }
        catch
        {
            // ReSharper disable once AccessToDisposedClosure Это backgroundService
            await channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);

            logger.LogCritical("Не удалось обработать сообщение с messageId {MessageId}", logData.MessageId);
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

    protected async Task<bool> TrySendToMediatorAsync<TCommand>(TCommand command)
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
