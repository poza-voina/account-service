using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Api.Features.Statements.GetStatement;
using AccountService.Api.Features.Transactions.ExecuteTransaction;
using AccountService.Api.Features.Transactions.TransferTransaction;
using AccountService.Api.ViewModels;
using AccountService.Infrastructure.Enums;
using AccountService.IntegrationTests.Base;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;
using AccountResponse = AccountService.Api.ViewModels.Result.MbResult<AccountService.Api.ViewModels.AccountViewModel>;
using StatementResponse = AccountService.Api.ViewModels.Result.MbResult<AccountService.Api.ViewModels.AccountWithTransactionsViewModel>;

namespace AccountService.IntegrationTests;

public class ParallelTransferTests(PostgresSqlFixture fixture, ITestOutputHelper output) : ControllerTestsBase, IClassFixture<PostgresSqlFixture>
{
    // ReSharper disable once StringLiteralTypo
    private IsolatedClientOptions DefaultIsolatedClientOptions { get; } = new() { ContainerFixture = fixture, PathToEnvironment = "TestConfigs/appsettings.test.json" };

    [Theory]
    [InlineData(50)]
    public async Task ShouldHandleParallelTasksSuccessfully(int count)
    {
        // Arrange
        var client = CreateIsolatedClient(DefaultIsolatedClientOptions);
        const string path = "/accounts";
        const string pathToMoney = "/transactions/execute";
        const string pathToStatement = "/statements/{0}?ownerId={1}";
        const string pathToTransfer = "/transactions/transfer";
        const int expectedBalance = 20000;

        var account = new CreateAccountCommand
        {
            OwnerId = ClientsIds.ElementAt(0),
            Type = AccountType.Checking,
            Currency = "USD"
        };

        var createAccountRequest1 = new HttpRequestBuilder(HttpMethod.Post, path)
            .WithJsonContent(account)
            .Build();

        account.OwnerId = ClientsIds.ElementAt(1);

        var createAccountRequest2 = new HttpRequestBuilder(HttpMethod.Post, path)
            .WithJsonContent(account)
            .Build();

        var responseAccount1 = await client.SendAsync(createAccountRequest1);
        var responseAccount2 = await client.SendAsync(createAccountRequest2);

        List<AccountViewModel> accounts =
            [
                (await responseAccount1.Content.ReadFromJsonAsync<AccountResponse>(DefaultSerializerOptions))!.Result ?? throw new InvalidOperationException(),
                (await responseAccount2.Content.ReadFromJsonAsync<AccountResponse>(DefaultSerializerOptions))!.Result ?? throw new InvalidOperationException()
            ];

        var transaction = new ExecuteTransactionCommand
        {
            BankAccountId = accounts.ElementAt(0).Id,
            // ReSharper disable once PossibleLossOfFraction Хочется делить на два, так как две операции пополнения
            Amount = expectedBalance / 2,
            Currency = "USD",
            Type = TransactionType.Debit,
            Description = "test",
            BankAccountVersion = accounts.ElementAt(0).Version
        };

        var addMoneyToAccount1Request = new HttpRequestBuilder(HttpMethod.Post, pathToMoney)
            .WithJsonContent(transaction)
            .Build();

        transaction.BankAccountId = accounts.ElementAt(1).Id;
        transaction.BankAccountVersion = accounts.ElementAt(1).Version;

        var addMoneyToAccount2Request = new HttpRequestBuilder(HttpMethod.Post, pathToMoney)
            .WithJsonContent(transaction)
            .Build();

        await client.SendAsync(addMoneyToAccount1Request);
        await client.SendAsync(addMoneyToAccount2Request);

        //Act
        var tasks = new List<Task<int>>();

        for (var i = 0; i < count; i++)
        {
            var fromIndex = i % 2 == 0 ? 0 : 1;
            var toIndex = 1 - fromIndex;

            var i1 = i;
            tasks.Add
                (
                    Task.Run
                    (async () =>
                        {
                            try
                            {
                                var getAccountFromIndexQuery = new GetStatementQuery { AccountId = accounts.ElementAt(fromIndex).Id, OwnerId = accounts.ElementAt(fromIndex).OwnerId };
                                var getAccountToIndexQuery = new GetStatementQuery { AccountId = accounts.ElementAt(toIndex).Id, OwnerId = accounts.ElementAt(toIndex).OwnerId };

                                var getAccountFromIndexRequest = new HttpRequestBuilder(HttpMethod.Get, string.Format(pathToStatement, getAccountFromIndexQuery.AccountId, getAccountFromIndexQuery.OwnerId))
                                    .Build();
                                var getAccountToIndexRequest = new HttpRequestBuilder(HttpMethod.Get, string.Format(pathToStatement, getAccountToIndexQuery.AccountId, getAccountToIndexQuery.OwnerId))
                                    .Build();

                                var getAccountFromIndexResponse = await client.SendAsync(getAccountFromIndexRequest);
                                var getAccountToIndexResponse = await client.SendAsync(getAccountToIndexRequest);

                                var getAccountFromIndexVersion = (await getAccountFromIndexResponse.Content.ReadFromJsonAsync<AccountResponse>(DefaultSerializerOptions))!.Result!.Version;
                                var getAccountToIndexVersion = (await getAccountToIndexResponse.Content.ReadFromJsonAsync<AccountResponse>(DefaultSerializerOptions))!.Result!.Version;

                                var transferCommand = new TransferTransactionCommand
                                {
                                    BankAccountId = accounts.ElementAt(fromIndex).Id,
                                    BankAccountVersion = getAccountFromIndexVersion,
                                    CounterpartyBankAccountId = accounts.ElementAt(toIndex).Id,
                                    CounterpartyBankAccountVersion = getAccountToIndexVersion,
                                    Amount = 10,
                                    Currency = "USD",
                                    Description = $"iter = {i1}"
                                };

                                var transferRequest = new HttpRequestBuilder(HttpMethod.Post, pathToTransfer)
                                    .WithJsonContent(transferCommand)
                                    .Build();

                                var transferResponse = await client.SendAsync(transferRequest);

                                return (int)transferResponse.StatusCode;
                            }
                            catch
                            {
                                return -1;
                            }

                        }
                    )
                );
        }

        // Assert
        var results = await Task.WhenAll(tasks);
        var statusCounts = results
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, y => y.Count());

        foreach (var kvp in statusCounts)
        {
            output.WriteLine($"Status {kvp.Key}: {kvp.Value} times");
        }

        statusCounts[200].Should().BeGreaterThan(0);

        var accountResults = accounts.Select(async x =>
        {
            var request = new HttpRequestBuilder(HttpMethod.Get, string.Format(pathToStatement, x.Id, x.OwnerId)).Build();
            var response = await client.SendAsync(request);
            var data = await response.Content.ReadFromJsonAsync<StatementResponse>(DefaultSerializerOptions);
            return data!.Result;
        });

        var actualAccounts = await Task.WhenAll(accountResults);

        var actualAccount1 = actualAccounts.ElementAt(0)!;
        var actualAccount2 = actualAccounts.ElementAt(1)!;

        (actualAccount1.Balance + actualAccount2.Balance).Should().Be(expectedBalance);
        var count1 = actualAccount1.Transactions.Count();
        var count2 = actualAccount2.Transactions.Count();

        count1.Should().BeGreaterThan(0);
        count2.Should().BeGreaterThan(0);
        count1.Should().Be(count2);
    }
}