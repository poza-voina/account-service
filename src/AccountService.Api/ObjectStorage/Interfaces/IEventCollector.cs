using AccountService.Api.ObjectStorage.Events;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IEventCollector
{
    IEnumerable<IEvent> GetEvents();
    void AddEvent(IEvent @event);
}
