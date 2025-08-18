using DotNet.Testcontainers.Containers;
using Xunit;

namespace AccountService.IntegrationTests.Base;

public interface IContainerFixture : IAsyncLifetime
{
    IContainer Container { get; }
    Task WaitForReady();
}

public interface IPostgresqlContainterFixture : IContainerFixture
{
    string ConnectionString { get; }
}

public interface IRabbitMqContainerFixture : IContainerFixture
{
    string Password { get; }
    string Username { get; }
    string Hostname { get; }
    int Port { get; }
    int ManagerPort { get; }
}