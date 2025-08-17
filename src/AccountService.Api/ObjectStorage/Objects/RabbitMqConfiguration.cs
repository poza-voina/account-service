namespace AccountService.Api.ObjectStorage.Objects;

public class RabbitMqConfiguration
{
    private readonly Dictionary<string, string> _bindings = new();
    public required string HostName { get; init; }
    public required int Port { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string Exchange { get; init; }
    public List<RabbitMqQueue> Queues { get; set; } = [];

    public RabbitMqConfiguration Map<T>(string routingKey)
    {
        _bindings[typeof(T).Name] = routingKey;
        return this;
    }

    public Dictionary<string, string> GetBindings() => _bindings;
}