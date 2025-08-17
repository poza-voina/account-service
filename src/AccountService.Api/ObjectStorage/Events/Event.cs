using MediatR;
using Microsoft.Extensions.Logging;

namespace AccountService.Api.ObjectStorage.Events;

public class Event<TPayload> : IEvent<TPayload>
{
    public required Guid EventId { get; set; }
    public required string OccuratedAt { get; set; }
    public required EventMeta Meta { get; set; }
    public TPayload? Payload { get; set; }
    public required string EventType { get; set; }

    public Type GetPayloadType() => typeof(TPayload);
}

public interface IEvent<TPayload> : IEventBase
{
    TPayload? Payload { get; }
}

public interface IEventBase : INotification
{
    Guid EventId { get; set; }
    string OccuratedAt { get; set; }
    EventMeta Meta { get; set; }
    Type GetPayloadType();
    string EventType { get; set; }
}