namespace AccountService.Api.ObjectStorage.Events;

public class Event<T>
{
    public required Guid EventId { get; set; }
    public required string OccuratedAt { get; set; }
    public required EventMeta Meta { get; set; }
    public T? Payload { get; set; }
}