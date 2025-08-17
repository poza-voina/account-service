namespace AccountService.Infrastructure.Models;

public class InboxDeadLetter : IDatabaseModel
{
    public Guid Id { get; set; }
    public string? EventType { get; set; }
    public string? Payload { get; set; }
    public DateTime FailedAt { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? StackTrace { get; set; }
}
