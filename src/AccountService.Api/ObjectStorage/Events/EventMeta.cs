namespace AccountService.Api.ObjectStorage.Events;

public class EventMeta
{
    public required string Version { get; set; }
    public required string Source { get; set; }
    public Guid? CorrelationId { get; set; }
    public required Guid CausationId { get; set; }
}