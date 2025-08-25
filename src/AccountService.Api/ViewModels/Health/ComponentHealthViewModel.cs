namespace AccountService.Api.ViewModels.Health;

/// <summary>
/// Представляет отдельный компонент системы для проверки состояния (health).
/// </summary>
public class ComponentHealthViewModel
{
    /// <summary>
    /// Имя компонента (например, "RabbitMQ", "Database").
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Статус компонента (UP, WARN, DOWN).
    /// </summary>
    public HealthStatus Status { get; set; }

    /// <summary>
    /// Дополнительная информация о компоненте (например, детали ошибки или количество сообщений в Outbox).
    /// </summary>
    // ReSharper disable once UnusedMember.Global Нужно
    public string? Details { get; set; }
}