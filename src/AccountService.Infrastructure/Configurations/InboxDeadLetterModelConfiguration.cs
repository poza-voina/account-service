using AccountService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Configurations;

public class InboxDeadLetterModelConfiguration : IEntityTypeConfiguration<InboxDeadLetter>
{
    public void Configure(EntityTypeBuilder<InboxDeadLetter> builder)
    {
        builder
            .ToTable("inboxDeadLetter");

        builder
            .HasKey(x => x.Id);

        ConfigureProperties(builder);
    }

    private static void ConfigureProperties(EntityTypeBuilder<InboxDeadLetter> builder)
    {
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder
            .Property(x => x.EventType)
            .HasMaxLength(100)
            .HasColumnName("eventType");

        builder
            .Property(x => x.Payload)
            .HasColumnName("payload");

        builder
            .Property(x => x.ExceptionMessage)
            .HasColumnName("exceptionMessage");

        builder
            .Property(x => x.StackTrace)
            .HasColumnName("stackTrace");

        builder
            .Property(x => x.FailedAt)
            .HasColumnName("failedAt")
            .HasDefaultValueSql("now() at time zone 'utc'")
            .IsRequired();
    }
}