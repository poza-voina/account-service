namespace AccountService.Api.ViewModels.Health;


/// <summary>
/// Представляет общий результат health‑чека сервиса с состоянием всех компонентов.
/// </summary>
public class HealthCheckViewModel
{
    /// <summary>
    /// Общий статус сервиса (UP, WARN, DOWN), вычисляемый по состоянию всех компонентов.
    /// </summary>
    public HealthStatus Status { get; set; }

    /// <summary>
    /// Список всех компонентов и их статусов.
    /// </summary>
    public IEnumerable<ComponentHealthViewModel> Components { get; set; } = [];
}