using AccountService.Api.ObjectStorage.Events;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IRabbitMqService
{
    // ReSharper disable once UnusedMember.Global Может понадобиться, если отправлять event в через контракт
    Task PublishAsync<T>(Guid messageId, T message, CancellationToken cancellationToken = default) where T : IEventBase;
    Task PublishAsync(Guid messageId, string @event, string message, CancellationToken cancellationToken = default);
}
