using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Npgsql;

namespace AccountService.IntegrationTests.Base;

public class PostgresSqlFixture : IContainerFixture
{
    private readonly IContainer _container = new ContainerBuilder()
        .WithImage("postgres:15")
        .WithEnvironment("POSTGRES_DB", "database")
        .WithEnvironment("POSTGRES_USER", "postgres")
        .WithEnvironment("POSTGRES_PASSWORD", "password")
        .WithPortBinding(5432, true)
        .Build();

    public string ConnectionString => $"Host=localhost;Port={_container.GetMappedPublicPort(5432)};Username=postgres;Password=password;Database=database";

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await WaitForPostgresReady();
    }

    private async Task WaitForPostgresReady()
    {
        var retries = 10;
        while (retries-- > 0)
        {
            try
            {
                await using var conn = new NpgsqlConnection(ConnectionString);
                await conn.OpenAsync();
                return;
            }
            catch
            {
                await Task.Delay(500);
            }
        }

        throw new Exception("PostgresSQL не готов после нескольких попыток.");
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
    }
}