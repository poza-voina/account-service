using AccountService.Api.ObjectStorage.Objects;
using RabbitMQ.Client;

namespace AccountService.Api.ObjectStorage;

public class RabbitMqInitializer(RabbitMqConfiguration config) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory { HostName = config.HostName, Port = config.Port, UserName = config.UserName, Password = config.Password};
        await using var connection = await factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(config.Exchange, "topic", cancellationToken: cancellationToken);

        foreach (var q in config.Queues)
        {
            await channel.QueueDeclareAsync(q.Name, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await channel.QueueBindAsync(q.Name, config.Exchange, q.RoutingKey, cancellationToken: cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}