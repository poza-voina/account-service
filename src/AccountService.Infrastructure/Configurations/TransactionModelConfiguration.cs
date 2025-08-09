using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Configurations;

public class TransactionModelConfiguration : IEntityTypeConfiguration<TransactionModel>
{
    public void Configure(EntityTypeBuilder<TransactionModel> builder)
    {
        builder
            .ToTable("transactions");

        builder
            .HasKey(x => x.Id);

        ConfigureProperties(builder);
    }

    private static void ConfigureProperties(EntityTypeBuilder<TransactionModel> builder)
    {
        builder
           .Property(x => x.Id)
           .HasColumnName("id");

        builder
            .Property(x => x.BankAccountId)
            .HasColumnName("bankAccountId");

        builder
            .Property(x => x.CounterpartyBankAccountId)
            .HasColumnName("counterpartyBankAccountId");

        builder
            .Property(x => x.Amount)
            .HasColumnName("amount");

        builder
            .Property(x => x.Currency)
            .HasColumnName("currency");

        builder
            .Property(x => x.Type)
            .HasColumnName("type")
            .HasConversion<string>();

        builder
            .Property(x => x.Description)
            .HasColumnName("description");

        builder
            .Property(x => x.IsApply)
            .HasColumnName("isApply");

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("createdAt");
    }
}