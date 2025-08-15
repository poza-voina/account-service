using AccountService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using JetBrains.Annotations;

namespace AccountService.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    
    [UsedImplicitly]
    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IConcurrencyModel).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(IConcurrencyModel.Version))
                    .IsRowVersion()
                    .IsConcurrencyToken();
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
