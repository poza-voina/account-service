using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Api.Features.Statements.GetStatement;
using AccountService.Api.Features.Transactions.TransferTransaction;
using AccountService.Api.ObjectStorage.Events;
using AccountService.Api.ObjectStorage.Events.Consumed;
using AccountService.Api.ViewModels;
using AccountService.Infrastructure.Enums;
using AccountService.IntegrationTests.Base;
using FluentAssertions;
using RabbitMQ.Client;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;
using AccountResponse = AccountService.Api.ViewModels.Result.MbResult<AccountService.Api.ViewModels.AccountViewModel>;

namespace AccountService.IntegrationTests;

public class RabbitMqIntegrationTests(PostgresSqlFixture postgresFixture, RabbitMqFixture rabbitmqFixture) : ControllerTestsBase, IClassFixture<PostgresSqlFixture>, IClassFixture<RabbitMqFixture>
{
    private IsolatedClientOptions DefaultIsolatedClientOptions { get; } = new() { RabbitMqContainerFixture = rabbitmqFixture, PostgresContainerFixture = postgresFixture, PathToEnvironment = "TestConfigs/appsettings.test.json" };

    [Fact]
    public async Task AccountBlockedEvent_WhenAccountFrozen_ShouldReturn409()
    {
        var client = CreateIsolatedClient(DefaultIsolatedClientOptions);
        const string path = "/accounts";
        const string pathToStatement = "/statements/{0}?ownerId={1}";
        const string pathToTransfer = "/transactions/transfer";

        var createAccountCommand = new CreateAccountCommand
        {
            OwnerId = ClientsIds.ElementAt(0),
            Type = AccountType.Checking,
            Currency = "USD"
        };

        var createAccountRequest1 = new HttpRequestBuilder(HttpMethod.Post, path)
            .WithJsonContent(createAccountCommand)
            .Build();

        createAccountCommand.OwnerId = ClientsIds.ElementAt(1);

        var createAccountRequest2 = new HttpRequestBuilder(HttpMethod.Post, path)
            .WithJsonContent(createAccountCommand)
            .Build();

        var responseAccount1 = await client.SendAsync(createAccountRequest1);
        var responseAccount2 = await client.SendAsync(createAccountRequest2);

        List<AccountViewModel> accounts =
            [
                (await responseAccount1.Content.ReadFromJsonAsync<AccountResponse>(DefaultSerializerOptions))!.Result ?? throw new InvalidOperationException(),
                (await responseAccount2.Content.ReadFromJsonAsync<AccountResponse>(DefaultSerializerOptions))!.Result ?? throw new InvalidOperationException()
            ];


        var factory = new ConnectionFactory
        {
            HostName = rabbitmqFixture.Hostname,
            Port = rabbitmqFixture.Port,
            UserName = rabbitmqFixture.Username,
            Password = rabbitmqFixture.Password
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        var account = accounts[0];

        var eventId = Guid.NewGuid();
        var body = new Event<ClientBlocked>
        {
            EventId = eventId,
            OccuratedAt = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
            Meta = new EventMeta
            {
                Version = "v1",
                Source = "lorem ipsum",
                CorrelationId = Guid.NewGuid(),
                CausationId = Guid.NewGuid()
            },
            Payload = new ClientBlocked
            {
                EventId = eventId,
                OccurredAt = DateTime.Now,
                ClientId = account.OwnerId
            },
            EventType = nameof(ClientBlocked)
        };

        var props = new BasicProperties
        {
            MessageId = Guid.NewGuid().ToString(),
            ContentType = "application/json"
        };

        await channel.BasicPublishAsync(
            exchange: "account.events",
            routingKey: "client.blocked",
            body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(body)),
            basicProperties: props,
            mandatory: false
        );

        await Task.Delay(10000);

        var getAccountFirstQuery = new GetStatementQuery { AccountId = accounts.ElementAt(0).Id, OwnerId = accounts.ElementAt(0).OwnerId };
        var getAccountSecondQuery = new GetStatementQuery { AccountId = accounts.ElementAt(1).Id, OwnerId = accounts.ElementAt(1).OwnerId };

        var getAccountFirstRequest = new HttpRequestBuilder(HttpMethod.Get, string.Format(pathToStatement, getAccountFirstQuery.AccountId, getAccountFirstQuery.OwnerId))
            .Build();
        var getAccountSecondRequest = new HttpRequestBuilder(HttpMethod.Get, string.Format(pathToStatement, getAccountSecondQuery.AccountId, getAccountSecondQuery.OwnerId))
            .Build();

        var getAccountFirstResponse = await client.SendAsync(getAccountFirstRequest);
        var getAccountSecondResponse = await client.SendAsync(getAccountSecondRequest);

        var getAccountFirstVersion = (await getAccountFirstResponse.Content.ReadFromJsonAsync<AccountResponse>(DefaultSerializerOptions))!.Result!.Version;
        var getAccountSecondVersion = (await getAccountSecondResponse.Content.ReadFromJsonAsync<AccountResponse>(DefaultSerializerOptions))!.Result!.Version;

        var transferCommand = new TransferTransactionCommand
        {
            BankAccountId = accounts.ElementAt(0).Id,
            BankAccountVersion = getAccountFirstVersion,
            CounterpartyBankAccountId = accounts.ElementAt(1).Id,
            CounterpartyBankAccountVersion = getAccountSecondVersion,
            Amount = 10,
            Currency = "USD",
            Description = "test"
        };

        var transferRequest = new HttpRequestBuilder(HttpMethod.Post, pathToTransfer)
            .WithJsonContent(transferCommand)
            .Build();

        var transferResponse = await client.SendAsync(transferRequest);

        transferResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
