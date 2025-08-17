using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Brokers.CreateDeadLetter;
using AccountService.Api.Features.Brokers.CreateInboxConsumed;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace AccountService.Api.ObjectStorage.Events.Consumers;

public abstract class ConsumerBase(
    RabbitMqConfiguration rabbitMqConfiguration,
    ConsumerConfiguration consumerConfiguration,
    IServiceProvider serviceProvider)
    : BackgroundService
{
    private const string EventTypePropertyName = "EventType";
    private const string EventIdPropertyName = "EventId";

    protected abstract Task HandleMessageAsync(string eventType, string message, CancellationToken stoppingToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = rabbitMqConfiguration.HostName,
            Port = rabbitMqConfiguration.Port,
            UserName = rabbitMqConfiguration.UserName,
            Password = rabbitMqConfiguration.Password
        };

        await using var connection = await factory.CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync();

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
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            Guid messageId = GetMessageId(message, ea.BasicProperties.MessageId);

            try
            {
                var eventType = ProcessProperties(message);

                await CheckMessageIdAsync(messageId, message);

                await HandleMessageAsync(eventType, message, stoppingToken);

                var command = new CreateInboxConsumedCommand
                {
                    MessageId = messageId,
                    Handler = nameof(ConsumerBase)
                };

                await TrySendToMediatorAsync(command);

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


                await channel.BasicNackAsync(ea.DeliveryTag, false, false, stoppingToken);
            }
            catch
            {
                await channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(
            queue: consumerConfiguration.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
        );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private Guid GetMessageId(string message, string? messageId)
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
        using (var scope = serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<InboxConsumed>>();

            if (await repository.GetAll().AnyAsync(x => x.MessageId == messageId))
            {
                throw new ConsumerHandleMessageException(message, "Сообщение уже было принято");
            }
        }
    }

    private string ProcessProperties(string message)
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
            using (var scope = serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(command!);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
}
