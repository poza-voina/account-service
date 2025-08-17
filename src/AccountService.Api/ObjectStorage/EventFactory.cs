using AccountService.Api.ObjectStorage.Events;
using AccountService.Api.ObjectStorage.Interfaces;

namespace AccountService.Api.ObjectStorage;

public class EventFactory : IEventFactory
{
    public IEvent<TPayload> CreateEvent<TPayload>(TPayload data, string source) where TPayload : IEventPayload
    {
        return new Event<TPayload>
        {
            OccuratedAt = DateTime.UtcNow.ToString(),
            Payload = data,
            EventId = Guid.NewGuid(),
            Meta = new EventMeta
            {
                Version = "v1",
                Source = source,
                CausationId = Guid.NewGuid() //NOTE: ???
            }
        };
    }
}
