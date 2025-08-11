using AccountService.IntergrationTests.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccountService.IntergrationTests;

public abstract class ControllerTestsBase
{
    private const string SCHEMA_FORMAT = "test_{0}";

    public ICollection<Guid> ClientsIds { get; private set; } = [];

    public ControllerTestsBase()
    {
    }

    public JsonSerializerOptions DefaultSerializerOptions { get; } = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public HttpClient CreateIsolatedClient(IsolatedClientOptions isolatedClientOptions)
    {
        var factory = new CustomWebApplicationFactory(
            options =>
            {
                if (isolatedClientOptions.ContainerFixture is { })
                {
                    var schemaName = NewSchemaName;
                    options.ConnectionString = $"{isolatedClientOptions.ContainerFixture.ConnectionString};Search Path={schemaName}";
                    options.DatabaseSchemaName = schemaName;
                }
                if (isolatedClientOptions.PathToEnvironment is { })
                {
                    options.PathToEnvironment = isolatedClientOptions.PathToEnvironment;
                }
            });

        ClientsIds = factory.Services.GetRequiredService<ICollection<Guid>>();

        return factory.CreateClient();
    }

    public StringContent EmptyContent { get; } =
        new StringContent(string.Empty, Encoding.UTF8, "application/json");

    private string NewSchemaName =>
        string.Format(SCHEMA_FORMAT, Guid.NewGuid());

}
