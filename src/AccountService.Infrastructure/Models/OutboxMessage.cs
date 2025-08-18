using AccountService.Infrastructure.Enums;

namespace AccountService.Infrastructure.Models;

public class OutboxMessage : IDatabaseModel
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public string EventType { get; set; } = string.Empty;

    public string EventPayload { get; set; } = string.Empty;

    public Guid CorrelationId { get; set; }

    public int RetryCount { get; set; }

    public OutboxStatus Status { get; set; } = OutboxStatus.Pending;
}