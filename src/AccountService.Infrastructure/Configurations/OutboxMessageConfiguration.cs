using AccountService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder
            .ToTable("outboxMessages");

        builder
            .HasKey(x => x.Id);

        ConfigureProperties(builder);
    }

    private static void ConfigureProperties(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("now() at time zone 'utc'")
            .IsRequired();

        builder
            .Property(x => x.CorrelationId)
            .HasColumnName("correlationId")
            .IsRequired();

        builder
            .Property(x => x.RetryCount)
            .HasColumnName("retryCount")
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .Property(x => x.ProcessedAt)
            .HasColumnName("processedAt");

        builder
            .Property(x => x.EventType)
            .HasColumnName("eventType")
            .HasConversion<string>()
            .IsRequired();

        builder
            .Property(x => x.EventPayload)
            .HasColumnName("eventPayload")
            .IsRequired();

        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();
    }
}