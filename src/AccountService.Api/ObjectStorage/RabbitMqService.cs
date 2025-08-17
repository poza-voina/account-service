using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AccountService.Api.ObjectStorage;

public class RabbitMqService(
    RabbitMqConfiguration configuration,
    ILogger<RabbitMqService> logger)
    : IRabbitMqService
{
    public async Task PublishAsync(string @event, string message, CancellationToken cancellationToken = default)
    {
        if (!configuration.GetBindings().TryGetValue(@event, out var routingKey))
        {
            throw new InvalidOperationException($"Routing key not configured for {@event}");
        }

        await InternalPublishAsync(routingKey, message, cancellationToken);
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        if (!configuration.GetBindings().TryGetValue(nameof(T), out var routingKey))
        {
            throw new InvalidOperationException($"Routing key not configured for {nameof(T)}");
        }

        var json = JsonSerializer.Serialize(message);
        await InternalPublishAsync(routingKey, json, cancellationToken);
    }

    private async Task InternalPublishAsync(string routingKey, string message, CancellationToken cancellationToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.HostName,
                UserName = configuration.UserName,
                Password = configuration.Password
            };

            using var connection = await factory.CreateConnectionAsync(cancellationToken);
            using var channel = await connection.CreateChannelAsync();

            var body = Encoding.UTF8.GetBytes(message);
            var properties = new BasicProperties
            {
                Persistent = true,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                MessageId = Guid.NewGuid().ToString()
            };

            await channel.BasicPublishAsync(
                exchange: configuration.Exchange,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken);

            logger.LogDebug("Message {MessageId} published to {RoutingKey}", properties.MessageId, routingKey);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to publish message to {RoutingKey}", routingKey);
            throw;
        }
    }
}