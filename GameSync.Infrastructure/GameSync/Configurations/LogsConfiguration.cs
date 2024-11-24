using GameSync.Domain.GameSync.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameSync.Infrastructure.GameSync.Configurations;

public class LogsConfiguration : IEntityTypeConfiguration<LogEntity>
{
    public void Configure(EntityTypeBuilder<LogEntity> builder)
    {
        builder.HasKey(x => x.Id).HasName("id");

        builder.ToTable("logs", t => t.HasCheckConstraint("CK_Logs_Severity", "severity IN ('Trace', 'Debug', 'Info', 'Warning', 'Error', 'Critical')"));

        builder.Property(e => e.Id)
            .UseMySqlIdentityColumn()
            .HasColumnName("id");

        builder.Property(e => e.Severity)
            .HasMaxLength(32)
            .HasColumnName("severity");

        builder.Property(e => e.Data)
            .HasMaxLength(2048)
            .HasColumnName("data");

        builder.Property(e => e.Date)
            .HasColumnType("datetime")
            .HasColumnName("date")
            .HasDefaultValueSql("NOW()");
    }
}
