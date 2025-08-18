namespace AccountService.Infrastructure.Models;

public class InboxConsumed : IDatabaseModel
{
    public Guid MessageId { get; set; }
    public DateTime ProcessedAt { get; set; }
    public required string Handler { get; set; }
}