namespace AccountService.Api.ObjectStorage.Events;

public interface IEvent<out TPayload> : IEventBase
{
    TPayload? Payload { get; }
}
