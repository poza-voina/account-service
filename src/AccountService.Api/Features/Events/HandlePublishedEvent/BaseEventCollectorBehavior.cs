using AccountService.Api.ObjectStorage.Events;
using AccountService.Api.ObjectStorage.Interfaces;
using MediatR;

namespace AccountService.Api.Features.Events.HandlePublishedEvent;

public class HandlePublishedEventHandler<TEvent>(IEventCollector eventCollector) : INotificationHandler<TEvent> where TEvent : BaseEvent
{
    public Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        eventCollector.AddEvent(notification);
        return Task.CompletedTask;
    }
}