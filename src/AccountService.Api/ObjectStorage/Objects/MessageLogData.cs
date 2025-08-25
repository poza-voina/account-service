using System.Diagnostics;

namespace AccountService.Api.ObjectStorage.Objects;

public class MessageLogData
{
    public string? MessageId { get; set; }
    public string? EventId { get; set; }
    public string? Type { get; set; }
    public string? CorrelationId { get; set; }
    // ReSharper disable once UnusedMember.Global
    public string? Retry { get; set; }
    public string? Latency { get; set; }
    public string? Version { get; set; }
    public required Stopwatch Stopwatch { get; set; }
}
