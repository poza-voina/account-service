namespace AccountService.Api.ObjectStorage.Events.Published;

public class TransferCompleted : IEventPayload
{
    public required Guid SourceAccountId { get; set; }
    public required Guid DestinationAccountId { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required Guid TransferId { get; set; }
}
