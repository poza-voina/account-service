namespace AccountService.Api.ObjectStorage.Events.Published;

public class AccountOpened : IEventPayload
{
    public required Guid AccountId { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Currency { get; set; }
    public required string Type { get; set; }
}
