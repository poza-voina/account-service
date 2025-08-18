using AccountService.Api.ObjectStorage.Events;

namespace AccountService.Api.ObjectStorage.Interfaces;

public interface IEventCollector
{
    IEnumerable<IEventBase> GetEvents();
    void AddEvent(IEventBase @event);
}
