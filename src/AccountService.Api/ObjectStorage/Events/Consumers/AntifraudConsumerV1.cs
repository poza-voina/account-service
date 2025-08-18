using AccountService.Abstractions.Exceptions;
using AccountService.Api.Features.Account.BlockAccounts;
using AccountService.Api.Features.Account.UnblockAccounts;
using AccountService.Api.ObjectStorage.Events.Consumed;
using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using System.Text.Json;

namespace AccountService.Api.ObjectStorage.Events.Consumers;

public class AntifraudConsumerV1(
    IRabbitMqConnectionMonitor monitor,
    ConsumerConfiguration consumerConfiguration,
    ILogger<AntifraudConsumerV1> logger,
    IServiceProvider serviceProvider)
    : ConsumerBase<AntifraudConsumerV1>(monitor, consumerConfiguration, logger, serviceProvider)
{
    protected override async Task HandleMessageAsync(string eventType, string message, CancellationToken cancelationToken)
    {
        try
        {
            await ExecuteMessageAsync(eventType,message, cancelationToken);
        }
        catch (Exception ex)
        {
            throw new ConsumerHandleMessageException(eventType, message, ex.Message, ex);
        }
    }

    private async Task ExecuteMessageAsync(string eventType, string message, CancellationToken cancellationToken)
    {
        switch (eventType)
        {
            case nameof(ClientBlocked):
                var blocked = JsonSerializer.Deserialize<Event<ClientBlocked>>(message) ?? throw new InvalidOperationException("AntifraudConsumer не удалось серилизовать");
                var blockedPayload = blocked.Payload ?? throw new InvalidOperationException("Нет payload");
                CheckVersion(blocked);

                await TrySendToMediatorAsync(new BlockAccountsCommand { OwnerId = blockedPayload.ClientId });
                break;

            case nameof(ClientUnblocked):
                var unblocked = JsonSerializer.Deserialize<Event<ClientUnblocked>>(message) ?? throw new InvalidOperationException("AntifraudConsumer не удалось серилизовать");
                var unblockedPayload = unblocked.Payload ?? throw new InvalidOperationException("Нет payload");
                CheckVersion(unblocked);

                await TrySendToMediatorAsync(new UnblockAccountsCommand { OwnerId = unblockedPayload.ClientId });
                break;
        }
    }

    private static void CheckVersion<T>(IEvent<T> blocked) where T: IEventPayload
    {
        if (blocked.Meta.Version != "v1")
        {
            throw new InvalidOperationException("Неправильная версия");
        }
    }
}