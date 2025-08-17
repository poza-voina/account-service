using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using RabbitMQ.Client;

namespace AccountService.IntegrationTests.Base;

public class RabbitMqFixture : IRabbitMqContainerFixture
{
    public IContainer Container { get; } = new ContainerBuilder()
        .WithImage("rabbitmq:3-management")
        .WithEnvironment("RABBITMQ_DEFAULT_USER", "test")
        .WithEnvironment("RABBITMQ_DEFAULT_PASS", "test")
        .WithPortBinding(5672, true)
        .WithPortBinding(15672, true)
        .Build();

    public string Username => "test";
    public string Password => "test";
    public int Port => Container.GetMappedPublicPort(5672);
    public string Hostname => Container.Hostname;

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        await WaitForReady();
    }

    public async Task DisposeAsync()
    {
        await Container.StopAsync();
        await Container.DisposeAsync();
    }

    public async Task StartAsync() => await Container.StartAsync();
    public async Task StopAsync() => await Container.StopAsync();

    public async Task WaitForReady()
    {
        const int maxAttempts = 10;
        const int delayMs = 3000;

        var factory = new ConnectionFactory
        {
            HostName = Hostname,
            Port = Port,
            UserName = Username,
            Password = Password,
            RequestedConnectionTimeout = TimeSpan.FromSeconds(2)
        };

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                var connection = await factory.CreateConnectionAsync();
                var channel = await connection.CreateChannelAsync();

                channel.Dispose();
                connection.Dispose();
                return;
            }
            catch
            {
                if (attempt == maxAttempts)
                    throw;
                await Task.Delay(delayMs);
            }
        }

        throw new Exception("RabbitMq не готов после нескольких попыток.");
    }
}