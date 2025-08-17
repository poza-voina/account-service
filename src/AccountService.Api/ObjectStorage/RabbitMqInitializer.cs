using AccountService.Api.ObjectStorage.Objects;
using RabbitMQ.Client;

namespace AccountService.Api.ObjectStorage;

public class RabbitMqInitializer(RabbitMqConfiguration config) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory { HostName = config.HostName, Port = config.Port, UserName = config.UserName, Password = config.Password};
        using var connection = await factory.CreateConnectionAsync(cancellationToken);
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(config.Exchange, "topic");

        foreach (var q in config.Queues)
        {
            await channel.QueueDeclareAsync(q.Name, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueBindAsync(q.Name, config.Exchange, q.RoutingKey);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}