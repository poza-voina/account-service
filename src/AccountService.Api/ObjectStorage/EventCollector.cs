using AccountService.Api.ObjectStorage.Events;
using AccountService.Api.ObjectStorage.Interfaces;

namespace AccountService.Api.ObjectStorage;

public class EventCollector : IEventCollector
{
    private readonly List<IEventBase> events = [];

    public void AddEvent(IEventBase @event) =>
        events.Add(@event);

    public IEnumerable<IEventBase> GetEvents() =>
        events;
}
