namespace AccountService.Api.ViewModels.Result;

/// <summary>
/// Ошибка выполнения операции
/// </summary>
public class OperationError
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Трейс
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// Ошибка, которая выпала в процессе выполнения операции
    /// </summary>
    public string? ExceptionType { get; set; }
}
