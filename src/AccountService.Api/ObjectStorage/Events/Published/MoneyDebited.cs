namespace AccountService.Api.ObjectStorage.Events.Published;

public class MoneyDebited : IEventPayload
{
    public required Guid AccountId { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required Guid OperationId { get; set; }
    public required string Reason { get; set; }
}
