using AccountService.Abstractions.Constants;
using AccountService.Abstractions.Extensions;
using AccountService.Infrastructure;
using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MigrationRunner;

public class Program
{
    private static async Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .Build();

        var connectionSection = configuration.GetRequiredSection(EnvironmentConstants.ConnectionSection);
        var connectionString = connectionSection.GetRequiredValue<string>(EnvironmentConstants.DefaultConnectionStringKey);

        Console.WriteLine(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        await using var context = new ApplicationDbContext(optionsBuilder.Options);

        await context.Database.ExecuteSqlRawAsync("CREATE EXTENSION IF NOT EXISTS btree_gist;");

        Console.WriteLine("Applying migrations...");
        await context.Database.MigrateAsync();

        await SeedAsync(context);

        Console.WriteLine("Migrations applied successfully.");
    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!await context.Accounts.AnyAsync())
        {
            var accounts = new[]
            {
                new Account
                {
                    Id = Guid.Parse("65d8a7c5-d4d1-4277-9176-2c3bc509697f"),
                    OwnerId = Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
                    Type = AccountType.Checking,
                    Currency = "USD",
                    Balance = 200,
                    IsDeleted = false
                },
                new Account
                {
                    Id = Guid.Parse("9f3a9110-0fbf-4603-9fac-9cc8472d085c"),
                    OwnerId = Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
                    Type = AccountType.Checking,
                    Currency = "USD",
                    Balance = 200,
                    IsDeleted = false
                },
                new Account
                {
                    Id = Guid.Parse("808dddb0-3589-4bcc-afe1-22d635924537"),
                    OwnerId = Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
                    Type = AccountType.Deposit,
                    Currency = "USD",
                    Balance = 10,
                    InterestRate = 0.2m,
                    IsDeleted = false
                }  
            };

            await context.Accounts.AddRangeAsync(accounts);
            await context.SaveChangesAsync();
        }
    }
}