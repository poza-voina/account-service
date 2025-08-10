using AccountService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Configurations;

public class TransactionModelConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder
            .ToTable("transactions");

        builder
            .HasKey(x => x.Id);

        ConfigureProperties(builder);
        ConfigureRelations(builder);
    }

    private static void ConfigureProperties(EntityTypeBuilder<Transaction> builder)
    {
        builder
           .Property(x => x.Id)
           .HasColumnName("id")
           .IsRequired();

        builder
            .Property(x => x.BankAccountId)
            .HasColumnName("bankAccountId")
            .IsRequired();

        builder
            .Property(x => x.CounterpartyBankAccountId)
            .HasColumnName("counterpartyBankAccountId");

        builder
            .Property(x => x.Amount)
            .HasColumnName("amount")
            .IsRequired();

        builder
            .Property(x => x.Currency)
            .HasColumnName("currency")
            .IsRequired();

        builder
            .Property(x => x.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired();

        builder
            .Property(x => x.IsApply)
            .HasColumnName("isApply")
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("now() at time zone 'utc'")
            .IsRequired();
    }
    
    private static void ConfigureRelations(EntityTypeBuilder<Transaction> builder)
    {
        builder
            .HasOne(x => x.BankAccount)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.BankAccountId);
        
        builder
            .HasOne(x => x.CounterpartyBankAccount)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.CounterpartyBankAccountId);   
    }
}