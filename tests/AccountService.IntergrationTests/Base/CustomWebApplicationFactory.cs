using AccountService.Api;
using AccountService.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.IntegrationTests.Base;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private CustomWebApplicationFactoryOptions FactoryOptions { get; } = new();
    private bool _disposed;

    public CustomWebApplicationFactory(Action<CustomWebApplicationFactoryOptions>? options = null)
    {
        options?.Invoke(FactoryOptions);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        if (FactoryOptions.PathToEnvironment is not null)
        {
            builder.ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.Sources.Clear();
                configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(FactoryOptions.PathToEnvironment, optional: false, reloadOnChange: false);
            });
        }

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            if (FactoryOptions is { ConnectionString: not null, DatabaseSchemaName: not null })
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(FactoryOptions.ConnectionString));

                var serviceProvider = services.BuildServiceProvider();
                ExecuteInScope(context =>
                {
#pragma warning disable
                    context.Database.ExecuteSqlRaw($"CREATE SCHEMA IF NOT EXISTS \"{FactoryOptions.DatabaseSchemaName}\"");
                    context.Database.ExecuteSqlRaw("CREATE EXTENSION IF NOT EXISTS btree_gist;");
#pragma warning restore                    
                    context.Database.Migrate();
                }, serviceProvider);
            } 
            else
            {
                services.AddSingleton(new DbContextOptions<ApplicationDbContext>());
                services.AddScoped<ApplicationDbContext>();
            }

            services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && FactoryOptions is { ConnectionString: not null, DatabaseSchemaName: not null })
            {
                ExecuteInScope(context =>
                {
#pragma warning disable
                    context.Database.ExecuteSqlRaw($"DROP SCHEMA IF EXISTS \"{FactoryOptions.DatabaseSchemaName}\" CASCADE");
#pragma warning restore
                });
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    private void ExecuteInScope(Action<ApplicationDbContext> action, ServiceProvider? provider = null)
    {
        var serviceProvider = provider ?? Services;

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        action(context);
    }
}