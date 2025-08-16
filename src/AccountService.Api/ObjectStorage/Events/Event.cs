using MediatR;

namespace AccountService.Api.ObjectStorage.Events;

public class Event<TPayload> : BaseEvent
{
    public TPayload? Data { get; set; }

    public override object? Payload => Data;

    public override Type GetPayloadType() => typeof(TPayload);
}

public abstract class BaseEvent : IEvent, INotification
{
    public required Guid EventId { get; set; }
    public required string OccuratedAt { get; set; }
    public required EventMeta Meta { get; set; }
    public abstract object? Payload { get; }
    public abstract Type GetPayloadType();
}

public interface IEvent
{
    Guid EventId { get; set; }
    string OccuratedAt { get; set; }
    EventMeta Meta { get; set; }
    object? Payload { get; }
    Type GetPayloadType();
}