using AccountService.Api.ObjectStorage.Events;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IRabbitMqService
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : IEventBase;
    Task PublishAsync(string @event, string message, CancellationToken cancellationToken = default);
}
