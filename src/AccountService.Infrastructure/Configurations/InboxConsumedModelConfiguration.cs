using AccountService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Configurations;

public class InboxConsumedModelConfiguration : IEntityTypeConfiguration<InboxConsumed>
{
    public void Configure(EntityTypeBuilder<InboxConsumed> builder)
    {
        builder
            .ToTable("inboxConsumed");

        builder
            .HasKey(x => x.MessageId);

        ConfigureProperties(builder);
    }

    private static void ConfigureProperties(EntityTypeBuilder<InboxConsumed> builder)
    {
        builder
            .Property(x => x.MessageId)
            .HasColumnName("messageId")
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(x => x.Handler)
            .HasColumnName("handler")
            .IsRequired();

        builder
            .Property(x => x.ProcessedAt)
            .HasColumnName("failedAt")
            .HasDefaultValueSql("now() at time zone 'utc'")
            .IsRequired();
    }
}