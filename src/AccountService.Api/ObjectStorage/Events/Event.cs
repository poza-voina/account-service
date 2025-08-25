namespace AccountService.Api.ObjectStorage.Events;

public class Event<TPayload> : IEvent<TPayload>
{
    public required Guid EventId { get; set; }
    public required string OccurredAt  { get; set; }
    public required EventMeta Meta { get; set; }
    public TPayload? Payload { get; set; }
    public required string EventType { get; set; }

    public Type GetPayloadType() => typeof(TPayload);
}