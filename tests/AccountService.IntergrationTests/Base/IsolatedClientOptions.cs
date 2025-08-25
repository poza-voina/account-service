namespace AccountService.IntegrationTests.Base;

public class IsolatedClientOptions
{
    public string? PathToEnvironment { get; set; }
    public IPostgresqlContainterFixture? PostgresContainerFixture { get; set; } 
    public IRabbitMqContainerFixture? RabbitMqContainerFixture { get; set; } 
}
