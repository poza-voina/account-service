namespace AccountService.Api.ObjectStorage.Events.Consumed;

public class ClientUnblocked : IEventPayload
{
    public Guid EventId { get; init; }
    public DateTime OccurredAt { get; init; }
    public Guid ClientId { get; init; }
}