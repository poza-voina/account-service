using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AccountService.IntegrationTests.Base;

namespace AccountService.IntegrationTests;

public abstract class ControllerTestsBase
{
    private const string SchemaFormat = "test_{0}";

    public ICollection<Guid> ClientsIds { get; private set; } = [];

    public JsonSerializerOptions DefaultSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public HttpClient CreateIsolatedClient(IsolatedClientOptions isolatedClientOptions)
    {
        var factory = new CustomWebApplicationFactory(
            options =>
            {
                if (isolatedClientOptions.PostgresContainerFixture is not null)
                {
                    var schemaName = NewSchemaName;
                    options.ConnectionString = $"{isolatedClientOptions.PostgresContainerFixture.ConnectionString};Search Path={schemaName}";
                    options.DatabaseSchemaName = schemaName;
                }

                if (isolatedClientOptions.RabbitMqContainerFixture is not null)
                {
                    options.RabbitMqTestOptions = new RabbitMqTestOptions
                    {
                        Hostname = isolatedClientOptions.RabbitMqContainerFixture.Container.Hostname,
                        Port = isolatedClientOptions.RabbitMqContainerFixture.Port,
                        Password = isolatedClientOptions.RabbitMqContainerFixture.Password,
                        Username = isolatedClientOptions.RabbitMqContainerFixture.Username
                    };
                }

                if (isolatedClientOptions.PathToEnvironment is not null)
                {
                    options.PathToEnvironment = isolatedClientOptions.PathToEnvironment;
                }
            });

        ClientsIds = factory.Services.GetRequiredService<ICollection<Guid>>();

        return factory.CreateClient();
    }

    private static string NewSchemaName =>
        string.Format(SchemaFormat, Guid.NewGuid());

}
