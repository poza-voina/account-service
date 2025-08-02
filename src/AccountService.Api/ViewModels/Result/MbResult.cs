namespace AccountService.Api.ViewModels.Result;

public class MbResult<T>
{
    public T? Result { get; set; }
    public OperationError? OperationError { get; set; }
    public IEnumerable<Error>? ValidationErrors { get; set; }
    public int StatusCode { get; set; }
}