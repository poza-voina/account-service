namespace AccountService.Api.ObjectStorage.Events.Consumed;

public class ClientBlocked : IEventPayload
{
    public Guid EventId { get; init; }
    public DateTime OccurredAt { get; init; }
    public Guid ClientId { get; init; }
}
