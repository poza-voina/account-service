namespace AccountService.IntegrationTests.Base;

public class IsolatedClientOptions
{
    public string? PathToEnvironment { get; set; }
    public IContainerFixture? ContainerFixture { get; set; } 
}
