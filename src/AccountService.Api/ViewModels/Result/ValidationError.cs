namespace AccountService.Api.ViewModels.Result;

/// <summary>
/// Ошибка валидации
/// </summary>
public class ValidationError
{
    /// <summary>
    /// Поле, по которому не прошла валидация
    /// </summary>
    public required string Field { get; set; }

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public required string Message { get; set; }
}
