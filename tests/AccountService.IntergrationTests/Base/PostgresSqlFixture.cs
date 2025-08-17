using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Npgsql;
using Xunit.Abstractions;

namespace AccountService.IntegrationTests.Base;

public class PostgresSqlFixture : IPostgresqlContainterFixture
{
    private readonly IContainer _container = new ContainerBuilder()
        .WithImage("postgres:15")
        .WithEnvironment("POSTGRES_DB", "database")
        .WithEnvironment("POSTGRES_USER", "postgres")
        .WithEnvironment("POSTGRES_PASSWORD", "password")
        .WithPortBinding(5432, true)
        .Build();

    public int Port => _container.GetMappedPublicPort(5432);

    public string ConnectionString => $"Host=localhost;Port={Port};Username=postgres;Password=password;Database=database";

    public IContainer Container => _container;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await WaitForReady();
    }

    public async Task WaitForReady()
    {
        int maxAttempts = 10;
        int delayMs = 1000;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
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
        await _container.StopAsync();
        await _container.DisposeAsync();
    }
}