using AccountService.Api.ObjectStorage.Events;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AccountService.Api.ObjectStorage;

public class RabbitMqService(
    RabbitMqConfiguration configuration,
    ILogger<RabbitMqService> logger)
    : IRabbitMqService
{
    private const string MetaPropertyName = "Meta";
    private const string CorrelationIdPropertyName = "CorrelationId";
    private const string CausationIdPropertyName = "CausationId";

    public async Task PublishAsync(string @event, string message, CancellationToken cancellationToken = default)
    {
        if (!configuration.GetBindings().TryGetValue(@event, out var routingKey))
        {
            throw new InvalidOperationException($"Routing key not configured for {@event}");
        }

        var (correlationId, causationId) = ProcessProperties(message);

        await InternalPublishAsync(
            routingKey: routingKey,
            message: message,
            correlationId: correlationId,
            causationId: causationId,
            cancellationToken: cancellationToken);
    }

    private static (string CorrelationId, string CausationId) ProcessProperties(string message)
    {
        using var doc = JsonDocument.Parse(message);
        var root = doc.RootElement;

        var meta = root.GetProperty(MetaPropertyName);

        var correlationId = meta.GetProperty(CorrelationIdPropertyName).GetString() 
            ?? throw new InvalidOperationException($"{CorrelationIdPropertyName} не найдено");
        var causationId = meta.GetProperty(CausationIdPropertyName).GetString()
            ?? throw new InvalidOperationException($"{CausationIdPropertyName} не найдено");

        return (correlationId, causationId);
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T: IEventBase
    {
        if (!configuration.GetBindings().TryGetValue(nameof(T), out var routingKey))
        {
            throw new InvalidOperationException($"Routing key not configured for {nameof(T)}");
        }

        var json = JsonSerializer.Serialize(message);

        var correlationId = message.Meta.CorrelationId.ToString() ?? throw new InvalidOperationException($"{CorrelationIdPropertyName} не найдено");
        var causationId = message.Meta.CausationId.ToString();


        await InternalPublishAsync(
             routingKey: routingKey,
             message: json,
             correlationId: correlationId,
             causationId: causationId,
             cancellationToken: cancellationToken);
    }

    private async Task InternalPublishAsync(
        string routingKey,
        string message,
        string correlationId,
        string causationId, CancellationToken cancellationToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.HostName,
                UserName = configuration.UserName,
                Password = configuration.Password
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(message);
            var properties = new BasicProperties
            {
                Persistent = true,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                MessageId = Guid.NewGuid().ToString(),
                Headers = new Dictionary<string, object?>
                {
                    { "X-Correlation-Id", correlationId },
                    { "X-Causation-Id", causationId }
                }
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