namespace AccountService.Api.ObjectStorage.Events;

public interface IEvent<TPayload> : IEventBase
{
    TPayload? Payload { get; }
}
