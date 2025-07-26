using AccountService.Api.Domains;
using AccountService.Api.Domains.Enums;

namespace AccountService.Api;

public static class MockDependencyInjection
{
    public static void AddMock(this IServiceCollection services)
    {
        AddMockAccounts(services);
        AddMockClients(services);
    }

    private static void AddMockClients(IServiceCollection services)
    {
        services.AddSingleton<ICollection<Guid>>(provider =>
            [
                Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
                Guid.Parse("a5f5c3b2-1e74-4e6d-9c9d-8bfbec79a222"),
                Guid.Parse("9f8e7d6c-5b4a-3c2d-1e0f-1234567890ab"),
                Guid.Parse("abcdefab-cdef-abcd-efab-cdefabcdef12"),
            ]);
    }

    private static void AddMockAccounts(IServiceCollection services)
    {
        services.AddSingleton<ICollection<Account>>(provider =>
        {
            var accounts = new List<Account>
            {
                new ()
                {
                    Id = Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
                    OwnerId = Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
                    Type = AccountType.Checking,
                    Currency = "USD",
                    Balance = 1000.50m,
                    InterestRate = 0.01m,
                    OpeningDate = DateTime.UtcNow.AddYears(-2),
                    ClosingDate = null,
                    IsDeleted = false,
                    Transactions =
                    [
                        new ()
                        {
                            Id = Guid.NewGuid(),
                            BankAccountId = Guid.NewGuid(),
                            CounterpartyBankAccountId = Guid.NewGuid(),
                            Amount = 100,
                            Currency = "USD",
                            Type = TransactionType.Debit,
                            Description = "Initial deposit",
                            CreatedAt = DateTime.UtcNow.AddMonths(-3)
                        },
                        new ()
                        {
                            Id = Guid.NewGuid(),
                            BankAccountId = Guid.NewGuid(),
                            CounterpartyBankAccountId = Guid.NewGuid(),
                            Amount = -50,
                            Currency = "USD",
                            Type = TransactionType.Debit,
                            Description = "ATM withdrawal",
                            CreatedAt = DateTime.UtcNow.AddMonths(-2)
                        },
                        new ()
                        {
                            Id = Guid.NewGuid(),
                            BankAccountId = Guid.NewGuid(),
                            CounterpartyBankAccountId = Guid.NewGuid(),
                            Amount = 200,
                            Currency = "USD",
                            Type = TransactionType.Debit,
                            Description = "Received from friend",
                            CreatedAt = DateTime.UtcNow.AddMonths(-1)
                        }
                    ]
                },
                new ()
                {
                    Id = Guid.Parse("a5f5c3b2-1e74-4e6d-9c9d-8bfbec79a222"),
                    OwnerId = Guid.Parse("a5f5c3b2-1e74-4e6d-9c9d-8bfbec79a222"),
                    Type = AccountType.Credit,
                    Currency = "EUR",
                    Balance = 5000m,
                    InterestRate = 0.03m,
                    OpeningDate = DateTime.UtcNow.AddYears(-1),
                    ClosingDate = null,
                    IsDeleted = false,
                    Transactions = []
                }
            };

            return accounts;
        });
    }
}