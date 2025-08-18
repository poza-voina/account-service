namespace AccountService.Infrastructure.Models;

public class InboxDeadLetter : IDatabaseModel
{
    public Guid Id { get; set; }
    public string? EventType { get; set; }
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength Не знаю размера
    public string? Payload { get; set; }
    public DateTime FailedAt { get; set; }
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength Не знаю размера
    public string? ExceptionMessage { get; set; }
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength Не знаю размера
    public string? StackTrace { get; set; }
}
