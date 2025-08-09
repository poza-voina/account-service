using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Configurations;

public class AccountModelConfiguration : IEntityTypeConfiguration<AccountModel>
{
    public void Configure(EntityTypeBuilder<AccountModel> builder)
    {
        builder
            .ToTable("accounts");

        builder
            .HasKey(x => x.Id);

        ConfigureProperties(builder);
    }

    private static void ConfigureProperties(EntityTypeBuilder<AccountModel> builder)
    {
        builder
            .Property(x => x.Id)
            .HasColumnName("id");

        builder
            .Property(x => x.OwnerId)
            .HasColumnName("ownerId");

        builder
            .Property(x => x.Type)
            .HasColumnName("type");

        builder
            .Property(x => x.Currency)
            .HasColumnName("currency");

        builder
            .Property(x => x.Balance)
            .HasColumnName("balance");

        builder
            .Property(x => x.InterestRate)
            .HasColumnName("interestRate");

        builder
            .Property(x => x.OpeningDate)
            .HasColumnName("openingDate");

        builder
            .Property(x => x.ClosingDate)
            .HasColumnName("closingDate");

        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("isDeleted");
    }
}
