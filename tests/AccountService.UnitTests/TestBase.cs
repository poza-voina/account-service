using AccountService.Api;
using AccountService.Infrastructure;
using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AccountService.UnitTests;

public abstract class TestBase : IAsyncLifetime
{
    protected ServiceProvider ServiceProvider { get; private set; } = null!;

    public virtual async Task AddDataAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!await context.Accounts.AnyAsync())
        {
            await context.Accounts.AddRangeAsync(DefaultAccounts);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
        }
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
        services.AddLogging();

        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));

        services.AddAutoMapperConfiguration();

        services.AddMockClients();

        services.AddServices();
        services.AddRepositories();
        services.AddHelpers();

        services.AddValidationConfiguration();

        services.AddMediatrConfiguration();

        ReplaceServicesWithMocks(services);
    }

    protected virtual void ReplaceServicesWithMocks(IServiceCollection services)
    {
    }

    protected T GetService<T>() where T : notnull => ServiceProvider.GetRequiredService<T>();

    public Task InitializeAsync()
    {
        var services = new ServiceCollection();

        ConfigureTestServices(services);
        ServiceProvider = services.BuildServiceProvider();

        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await ServiceProvider.DisposeAsync();
    }

    public static readonly Guid[] Guids =
    [
        new("00000000-0000-0000-0000-000000000000"),
        new("11111111-1111-1111-1111-111111111111"),
        new("22222222-2222-2222-2222-222222222222"),
        new("33333333-3333-3333-3333-333333333333"),
        new("44444444-4444-4444-4444-444444444444"),
        new("55555555-5555-5555-5555-555555555555"),
        new("66666666-6666-6666-6666-666666666666"),
        new("77777777-7777-7777-7777-777777777777"),
        new("88888888-8888-8888-8888-888888888888")
    ];

    public Guid[] Guids => _guids;

    public IEnumerable<Account> DefaultAccounts { get; } = new List<Account>
    {
        new()
        {
            Id = Guids[0],
            OwnerId = Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
            Type = AccountType.Checking,
            Currency = "USD",
            Balance = 1000.00m,
            InterestRate = 1.5m,
            OpeningDate = DateTime.UtcNow.AddMonths(-3),
            ClosingDate = null,
            IsDeleted = false,
            Version = 1,
            Transactions = new List<Transaction>(),
            CounterPartyTransactions = new List<Transaction>()
        },
        new()
        {
            Id = Guids[1],
            OwnerId = Guid.Parse("a5f5c3b2-1e74-4e6d-9c9d-8bfbec79a222"),
            Type = AccountType.Deposit,
            Currency = "USD",
            Balance = 5000.00m,
            InterestRate = 2.0m,
            OpeningDate = DateTime.MinValue,
            ClosingDate = null,
            IsDeleted = false,
            Version = 3,
            Transactions = new List<Transaction>(),
            CounterPartyTransactions = new List<Transaction>()
        }
    };
}
