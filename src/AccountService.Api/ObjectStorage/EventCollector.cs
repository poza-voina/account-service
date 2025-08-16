using AccountService.Api.ObjectStorage.Events;
using AccountService.Api.ObjectStorage.Interfaces;

namespace AccountService.Api.ObjectStorage;

public class EventCollector : IEventCollector
{
    private readonly List<IEvent> events = [];

    public void AddEvent(IEvent @event) =>
        events.Add(@event);

    public IEnumerable<IEvent> GetEvents() =>
        events;
}
