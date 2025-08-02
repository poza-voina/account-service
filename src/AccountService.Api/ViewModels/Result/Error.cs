namespace AccountService.Api.ViewModels.Result;

public class Error
{
    public required string Field { get; set; }
    public required string Message { get; set; }
}
