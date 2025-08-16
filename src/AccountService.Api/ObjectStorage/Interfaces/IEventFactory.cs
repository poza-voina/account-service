using AccountService.Api.ObjectStorage.Events;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IEventFactory
{
    Event<TPayload> CreateEvent<TPayload>(TPayload data, string source) where TPayload : IEventPayload;
}
