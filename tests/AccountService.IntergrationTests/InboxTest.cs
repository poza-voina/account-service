using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Infrastructure.Enums;
using AccountService.IntegrationTests.Base;
using FluentAssertions;
using RabbitMQ.Client;
using System.Net;
using Xunit;

namespace AccountService.IntegrationTests;

public class OutboxTests(PostgresSqlFixture postgresFixture, RabbitMqFixture rabbitmqFixture) : ControllerTestsBase, IClassFixture<PostgresSqlFixture>, IClassFixture<RabbitMqFixture>
{
    // ReSharper disable once StringLiteralTypo так файл называется
    private IsolatedClientOptions DefaultIsolatedClientOptions { get; } = new() { RabbitMqContainerFixture = rabbitmqFixture, PostgresContainerFixture = postgresFixture, PathToEnvironment = "TestConfigs/appsettings.test.json" };

    [Fact]
    public async Task OutboxTest_WhenConnectionLost_MessagesSent() //НЕ СМОГ СДЕЛАТЬ ТЕСТ С ПАДЕНИЕМ КОНТЕЙНЕРА
    {        
        const string queueName = "account.crm";

        var client = CreateIsolatedClient(DefaultIsolatedClientOptions);
        const string path = "/accounts";

        var createAccountCommand = new CreateAccountCommand
        {
            OwnerId = ClientsIds.ElementAt(0),
            Type = AccountType.Checking,
            Currency = "USD"
        };

        foreach (var unused in ClientsIds)
        {
            var request = new HttpRequestBuilder(HttpMethod.Post, path)
                .WithJsonContent(createAccountCommand)
                .Build();

            var response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        await Task.Delay(50000);

        var factory = new ConnectionFactory
        {
            HostName = rabbitmqFixture.Hostname,
            Port = rabbitmqFixture.Port,
            UserName = rabbitmqFixture.Username,
            Password = rabbitmqFixture.Password
        };

        var count = 0;
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        while (true)
        {
            var result = await channel.BasicGetAsync(queueName, autoAck: false);
            if (result == null)
            {
                break;
            }

            count++;

            await channel.BasicAckAsync(result.DeliveryTag, multiple: false);
        }

        count.Should().BeGreaterThan(0);
    }
}
