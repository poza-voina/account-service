namespace AccountService.IntegrationTests.Base;

public class CustomWebApplicationFactoryOptions
{
    public string? PathToEnvironment { get; set; }

    public string? ConnectionString { get; set; }

    public string? DatabaseSchemaName { get; set; }

    public RabbitMqTestOptions? RabbitMqTestOptions { get; set; }
}

public class RabbitMqTestOptions
{
    public required string Hostname { get; set; }
    public required int Port { get; set; }
    public required string Password { get; set; }
    public required string Username { get; set; }
}