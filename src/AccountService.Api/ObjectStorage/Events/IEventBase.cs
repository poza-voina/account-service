using MediatR;

namespace AccountService.Api.ObjectStorage.Events;

public interface IEventBase : INotification
{
    Guid EventId { get; set; }
    string OccuratedAt { get; set; }
    EventMeta Meta { get; set; }
    Type GetPayloadType();
    string EventType { get; set; }
}