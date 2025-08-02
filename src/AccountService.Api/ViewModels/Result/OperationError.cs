namespace AccountService.Api.ViewModels.Result;

public class OperationError
{
    public required string Message { get; set; }
    public string? StackTrace { get; set; }
    public string? ExceptionType { get; set; }
}
