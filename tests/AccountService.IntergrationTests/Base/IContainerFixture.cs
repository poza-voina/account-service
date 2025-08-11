using Xunit;

namespace AccountService.IntergrationTests.Base;

public interface IContainerFixture : IAsyncLifetime
{
    public string ConnectionString { get; }
}