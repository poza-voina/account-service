using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Brokers.CreateDeadLetter;
using AccountService.Api.Features.Brokers.CreateInboxConsumed;
using AccountService.Api.ObjectStorage.Objects;
using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AccountService.Api.ObjectStorage.Events.Consumers;

public abstract class ConsumerBase(
    RabbitMqConfiguration rabbitMqConfiguration,
    ConsumerConfiguration consumerConfiguration,
    IServiceProvider serviceProvider)
    : BackgroundService
{
    protected abstract Task HandleMessageAsync(string eventType, string message, CancellationToken stoppingToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = rabbitMqConfiguration.HostName,
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

            try
            {
                var (eventType, messageId) = ProcessProperties(message);

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

    private async Task CheckMessageIdAsync(Guid messageId, string message)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<InboxConsumed>>();

            if (repository.GetAll().Any(x => x.MessageId == messageId))
            {
                throw new ConsumerHandleMessageException(message, "Сообщение уже было принято");
            }
        }
    }

    private (string EventType, Guid MessageId) ProcessProperties(string message)
    {
        using var doc = JsonDocument.Parse(message);
        var root = doc.RootElement;

        JsonElement typeProperty;
        JsonElement eventId;
        if (!root.TryGetProperty("EventType", out typeProperty) ||
            !root.TryGetProperty("EventId", out eventId))
        {
            throw new ConsumerHandleMessageException(message, "Не удалось найти EventType и EventId");
        }

        var eventType = typeProperty.GetString() ?? throw new ConsumerHandleMessageException(message, "Не удалось найти eventType");

        Guid messageId;
        try
        {
            messageId = eventId.GetGuid();
        }
        catch (Exception ex)
        {
            throw new ConsumerHandleMessageException(eventType, message, "Не удалось найти eventId", ex);
        }

        return (eventType, messageId);
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
