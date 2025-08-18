using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Account.BlockAccounts;
using AccountService.Api.Features.Account.UnblockAccounts;
using AccountService.Api.ObjectStorage.Events.Consumed;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AccountService.Api.ObjectStorage.Events.Consumers;

public class AntifraudConsumerV1(
    IRabbitMqConnectionMonitor monitor,
    ConsumerConfiguration consumerConfiguration,
    ILogger<AntifraudConsumerV1> logger,
    IServiceProvider serviceProvider)
    : ConsumerBase<AntifraudConsumerV1>(monitor, consumerConfiguration, logger, serviceProvider)
{
    protected override async Task HandleMessageAsync(string eventType, string message, MessageLogData logData, CancellationToken cancelationToken)
    {
        try
        {
            await ExecuteMessageAsync(eventType,message, logData, cancelationToken);
        }
        catch (Exception ex)
        {
            throw new ConsumerHandleMessageException(eventType, message, ex.Message, ex);
        }
    }

    private async Task ExecuteMessageAsync(string eventType, string message, MessageLogData logData, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        switch (eventType)
        {
            case nameof(ClientBlocked):
                var blocked = JsonSerializer.Deserialize<Event<ClientBlocked>>(message, options) ?? throw new InvalidOperationException("AntifraudConsumer failed to serialize");
                var blockedPayload = blocked.Payload ?? throw new InvalidOperationException("No payload");
                ProcessLogData(logData, blocked);
                CheckVersion(blocked);

                await TrySendToMediatorAsync(new BlockAccountsCommand { OwnerId = blockedPayload.ClientId });
                break;

            case nameof(ClientUnblocked):
                var unblocked = JsonSerializer.Deserialize<Event<ClientUnblocked>>(message, options) ?? throw new InvalidOperationException("AntifraudConsumer failed to serialize");
                var unblockedPayload = unblocked.Payload ?? throw new InvalidOperationException("No payload");
                ProcessLogData(logData, unblocked);
                CheckVersion(unblocked);

                await TrySendToMediatorAsync(new UnblockAccountsCommand { OwnerId = unblockedPayload.ClientId });
                break;
        }
    }

    private static void ProcessLogData<T>(MessageLogData logData, IEvent<T> message)
    {
        logData.EventId = message.EventId.ToString();
        logData.Type = message.EventType;
        logData.CorrelationId = message.Meta.CorrelationId.ToString();
        logData.Version = message.Meta.Version.ToString();
    }

    private static void CheckVersion<T>(IEvent<T> message) where T: IEventPayload
    {
        if (message.Meta.Version != "v1")
        {
            throw new InvalidOperationException("Invalid version");
        }
    }
}