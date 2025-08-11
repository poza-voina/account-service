using AccountService.Abstractions.Constants;
using AccountService.Abstractions.Extensions;
using AccountService.Infrastructure;
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

        Console.WriteLine("Applying migrations...");
        await context.Database.MigrateAsync();

        Console.WriteLine("Migrations applied successfully.");
    }
}