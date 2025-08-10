using AccountService.Abstractions.Constants;
using AccountService.Abstractions.Extensions;
using AccountService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .Build();

        var connectionSection = configuration.GetRequiredSection(EnvironmentContants.CONNECTION_SECTION);
        var connectionString = connectionSection.GetRequiredValue<string>(EnvironmentContants.DEFAULT_CONNECTION_STRING_KEY);

        Console.WriteLine(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        using var context = new ApplicationDbContext(optionsBuilder.Options);

        Console.WriteLine("Applying migrations...");
        await context.Database.MigrateAsync();

        Console.WriteLine("Migrations applied successfully.");
    }
}