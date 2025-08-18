using DotNet.Testcontainers.Containers;
using JetBrains.Annotations;
using Xunit;

namespace AccountService.IntegrationTests.Base;

public interface IContainerFixture : IAsyncLifetime
{
    IContainer Container { get; }
    // ReSharper disable once UnusedMemberInSuper.Global Используется
    Task WaitForReady();
}

// ReSharper disable once IdentifierTypo 
public interface IPostgresqlContainterFixture : IContainerFixture
{
    string ConnectionString { get; }
}

public interface IRabbitMqContainerFixture : IContainerFixture
{
    string Password { get; }
    string Username { get; }
    [UsedImplicitly]
    string Hostname { get; }
    int Port { get; }
    [UsedImplicitly]
    int ManagerPort { get; }
}