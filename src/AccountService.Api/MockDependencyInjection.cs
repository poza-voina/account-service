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
        services.AddSingleton<ICollection<Guid>>(_ =>
            [
                Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
                Guid.Parse("a5f5c3b2-1e74-4e6d-9c9d-8bfbec79a222"),
                Guid.Parse("9f8e7d6c-5b4a-3c2d-1e0f-1234567890ab"),
                Guid.Parse("abcdefab-cdef-abcd-efab-cdefabcdef12")
            ]);
    }

    private static void AddMockAccounts(IServiceCollection services)
    {
        services.AddSingleton<ICollection<Account>>(_ =>
        {
            var account1Id = Guid.Parse("c0d4f0d2-69e1-4b3e-9e1a-7e6c56d4c301");
            var account2Id = Guid.Parse("7c9fcf13-6df1-4eb1-9404-0e4380e2bba5");
            var account3Id = Guid.Parse("aa1c5fd0-3e12-4b3f-b7ab-2bcfcbaf7432");

            var guidsForPairs = new List<Guid>
            {
                Guid.Parse("fe9c4d27-c91c-45e7-a6a5-b2d5e2fc7a55"),
                Guid.Parse("8cdb7d19-5210-4cb0-a4f3-0b1cc1e8ea63"),
                Guid.Parse("60f5d2a3-7f47-41e3-8924-e7192aee6a31"),
                Guid.Parse("f70f9642-91ec-432a-92e4-0ff99e2cd7b1"),
                Guid.Parse("a746ea4f-97fc-4d89-a18e-f3cc45e1973e"),
                Guid.Parse("1185f69c-6db7-4638-b08d-637685a28932"),
                Guid.Parse("66ff7f75-5f87-441e-b194-74e649e21eae"),
                Guid.Parse("d6eaef24-e6ae-42e1-a7e9-babfb5c06d4f"),
                Guid.Parse("a2ef5357-fd26-4be3-9b11-2436a2698e9c")
            };

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
                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            BankAccountId = Guid.NewGuid(),
                            CounterpartyBankAccountId = Guid.NewGuid(),
                            Amount = 100,
                            Currency = "USD",
                            Type = TransactionType.Debit,
                            Description = "Initial deposit",
                            CreatedAt = DateTime.UtcNow.AddMonths(-3),
                            IsApply = true
                        },
                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            BankAccountId = Guid.NewGuid(),
                            CounterpartyBankAccountId = Guid.NewGuid(),
                            Amount = -50,
                            Currency = "USD",
                            Type = TransactionType.Debit,
                            Description = "ATM withdrawal",
                            CreatedAt = DateTime.UtcNow.AddMonths(-2),
                            IsApply = true
                        },
                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            BankAccountId = Guid.NewGuid(),
                            CounterpartyBankAccountId = Guid.NewGuid(),
                            Amount = 200,
                            Currency = "USD",
                            Type = TransactionType.Debit,
                            Description = "Received from friend",
                            CreatedAt = DateTime.UtcNow.AddMonths(-1),
                            IsApply = true
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
                },

                new ()
                {
                    Id = account1Id,
                    OwnerId = account1Id,
                    Type = AccountType.Checking,
                    Currency = "USD",
                    Balance = 1000.50m,
                    InterestRate = 0.01m,
                    OpeningDate = DateTime.UtcNow.AddYears(-2),
                    ClosingDate = null,
                    IsDeleted = false,
                    Transactions =
                    [
                        new Transaction
                        {
                            Id = guidsForPairs[0],
                            BankAccountId = account1Id,
                            CounterpartyBankAccountId = account2Id,
                            Amount = 200,
                            Currency = "USD",
                            Type = TransactionType.Credit,
                            Description = "Transfer to account2",
                            CreatedAt = DateTime.UtcNow.AddDays(-10),
                            IsApply = false
                        },
                        new Transaction
                        {
                            Id = guidsForPairs[1],
                            BankAccountId = account2Id,
                            CounterpartyBankAccountId = account1Id,
                            Amount = -200,
                            Currency = "USD",
                            Type = TransactionType.Debit,
                            Description = "Received from account1",
                            CreatedAt = DateTime.UtcNow.AddDays(-10),
                            IsApply = false
                        },
                        new Transaction
                        {
                            Id = guidsForPairs[2],
                            BankAccountId = account1Id,
                            CounterpartyBankAccountId = account3Id,
                            Amount = 150,
                            Currency = "EUR",
                            Type = TransactionType.Credit,
                            Description = "Loan to account3",
                            CreatedAt = DateTime.UtcNow.AddDays(-9),
                            IsApply = false
                        },
                        new Transaction
                        {
                            Id = guidsForPairs[3],
                            BankAccountId = account3Id,
                            CounterpartyBankAccountId = account1Id,
                            Amount = -150,
                            Currency = "EUR",
                            Type = TransactionType.Debit,
                            Description = "Loan from account1",
                            CreatedAt = DateTime.UtcNow.AddDays(-9),
                            IsApply = false
                        },
                        new Transaction
                        {
                            Id = guidsForPairs[4],
                            BankAccountId = account2Id,
                            CounterpartyBankAccountId = account3Id,
                            Amount = 300,
                            Currency = "USD",
                            Type = TransactionType.Credit,
                            Description = "Payment to account3",
                            CreatedAt = DateTime.UtcNow.AddDays(-7),
                            IsApply = false
                        },
                        new Transaction
                        {
                            Id = guidsForPairs[5],
                            BankAccountId = account3Id,
                            CounterpartyBankAccountId = account2Id,
                            Amount = -300,
                            Currency = "USD",
                            Type = TransactionType.Debit,
                            Description = "Payment from account2",
                            CreatedAt = DateTime.UtcNow.AddDays(-7),
                            IsApply = false
                        },
                        new Transaction
                        {
                            Id = guidsForPairs[6],
                            BankAccountId = account1Id,
                            CounterpartyBankAccountId = account2Id,
                            Amount = 75,
                            Currency = "GBP",
                            Type = TransactionType.Credit,
                            Description = "Gift to account2",
                            CreatedAt = DateTime.UtcNow.AddDays(-6),
                            IsApply = false
                        },
                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            BankAccountId = account2Id,
                            CounterpartyBankAccountId = account1Id,
                            Amount = -75,
                            Currency = "GBP",
                            Type = TransactionType.Debit,
                            Description = "Gift from account1",
                            CreatedAt = DateTime.UtcNow.AddDays(-6),
                            IsApply = false
                        },
                        new Transaction
                        {
                            Id = guidsForPairs[7],
                            BankAccountId = account1Id,
                            CounterpartyBankAccountId = Guid.NewGuid(),
                            Amount = 120,
                            Currency = "USD",
                            Type = TransactionType.Credit,
                            Description = "Unmatched incoming",
                            CreatedAt = DateTime.UtcNow.AddDays(-5),
                            IsApply = false
                        },
                        new Transaction
                        {
                            Id = guidsForPairs[8],
                            BankAccountId = account1Id,
                            CounterpartyBankAccountId = Guid.NewGuid(),
                            Amount = -60,
                            Currency = "USD",
                            Type = TransactionType.Debit,
                            Description = "Unmatched outgoing",
                            CreatedAt = DateTime.UtcNow.AddDays(-4),
                            IsApply = false
                        }
                    ]
                },
                new ()
                {
                    Id = account2Id,
                    OwnerId = account2Id,
                    Type = AccountType.Credit,
                    Currency = "USD",
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