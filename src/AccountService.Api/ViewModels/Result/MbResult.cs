namespace AccountService.Api.ViewModels.Result;

/// <summary>
/// Результат выполнения запроса
/// </summary>
/// <typeparam name="T">Тип запроса</typeparam>
public class MbResult<T>
{
    /// <summary>
    /// Результат выполнения запроса
    /// </summary>
    public T? Result { get; set; }
    
    /// <summary>
    /// Ошибка операции
    /// </summary>
    public OperationError? OperationError { get; set; }

    /// <summary>
    /// Ошибки валидации
    /// </summary>
    public IEnumerable<Error>? ValidationErrors { get; set; }

    /// <summary>
    /// Код статуса выполнения
    /// </summary>
    public int StatusCode { get; set; }
}