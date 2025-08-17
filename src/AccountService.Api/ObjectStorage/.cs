using AccountService.Api.ObjectStorage.Objects;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AccountService.Api.ObjectStorage;

public class RabbitMqInitializer : IHostedService
{
    private readonly RabbitMqConfiguration _config;
    private readonly ILogger<RabbitMqInitializer> _logger;

    public RabbitMqInitializer(RabbitMqConfiguration config, ILogger<RabbitMqInitializer> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory { HostName = _config.HostName, Port = _config.Port, UserName = _config.UserName, Password = _config.Password};
        using var connection = await factory.CreateConnectionAsync(cancellationToken);
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(_config.Exchange, "topic");

        foreach (var q in _config.Queues)
        {
            await channel.QueueDeclareAsync(q.Name, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueBindAsync(q.Name, _config.Exchange, q.RoutingKey);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

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