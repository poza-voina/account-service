using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Npgsql;

namespace AccountService.IntegrationTests.Base;

public class PostgresSqlFixture : IPostgresqlContainterFixture
{
    public int Port => Container.GetMappedPublicPort(5432);

    public string ConnectionString => $"Host=localhost;Port={Port};Username=postgres;Password=password;Database=database";

    public IContainer Container { get; } = new ContainerBuilder()
        .WithImage("postgres:15")
        .WithEnvironment("POSTGRES_DB", "database")
        .WithEnvironment("POSTGRES_USER", "postgres")
        .WithEnvironment("POSTGRES_PASSWORD", "password")
        .WithPortBinding(5432, true)
        .Build();

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        await WaitForReady();
    }

    public async Task WaitForReady()
    {
        const int maxAttempts = 10;
        const int delayMs = 1000;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await using var conn = new NpgsqlConnection(ConnectionString);
                await conn.OpenAsync();
                return;
            }
            catch
            {
                if (attempt == maxAttempts)
                    throw;
                await Task.Delay(delayMs);
            }
        }

        throw new Exception("PostgresSQL не готов после нескольких попыток.");
    }

    public async Task DisposeAsync()
    {
        await Container.StopAsync();
        await Container.DisposeAsync();
    }
}