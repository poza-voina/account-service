using Xunit;

namespace AccountService.IntegrationTests.Base;

public interface IContainerFixture : IAsyncLifetime
{
    public string ConnectionString { get; }
}