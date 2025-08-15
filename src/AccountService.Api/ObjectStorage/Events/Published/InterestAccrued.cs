namespace AccountService.Api.ObjectStorage.Events.Published;

public class InterestAccrued
{
    public required Guid AccountId { get; set; }
    public required DateTime PeriodFrom { get; set; }
    public required DateTime PeriodTo { get; set; }
    public required decimal Amount { get; set; }
}