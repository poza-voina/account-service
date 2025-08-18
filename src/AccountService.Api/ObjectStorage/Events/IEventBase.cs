using MediatR;

namespace AccountService.Api.ObjectStorage.Events;

public interface IEventBase : INotification
{
    Guid EventId { get; set; }
    // ReSharper disable once UnusedMember.Global Нужно по заданию
    // ReSharper disable once UnusedMemberInSuper.Global
    string OccurredAt { get; set; }
    EventMeta Meta { get; set; }
    Type GetPayloadType();
    string EventType { get; set; }
}