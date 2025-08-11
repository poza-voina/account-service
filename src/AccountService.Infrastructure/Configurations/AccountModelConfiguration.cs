using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AccountService.Infrastructure.Configurations;

public class AccountModelConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder
            .ToTable("accounts");

        builder
            .HasKey(x => x.Id);

        ConfigureProperties(builder);
        ConfigureIndexes(builder);
    }

    private static void ConfigureIndexes(EntityTypeBuilder<Account> builder)
    {
        builder
            .HasIndex(x => x.OwnerId)
            .HasMethod("hash");
    }

    private static void ConfigureProperties(EntityTypeBuilder<Account> builder)
    {
        builder
            .Property(x => x.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(x => x.OwnerId)
            .IsRequired()
            .HasColumnName("ownerId");

        builder
            .Property(x => x.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnName("type");

        builder
            .Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasColumnName("currency");

        builder
            .Property(x => x.Balance)
            .IsRequired()
            .HasColumnName("balance");

        builder
            .Property(x => x.InterestRate)
            .HasColumnName("interestRate");

        builder
            .Property(x => x.OpeningDate)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'")
            .HasColumnName("openingDate");

        builder
            .Property(x => x.ClosingDate)
            .HasColumnName("closingDate");

        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("isDeleted");
    }
}
