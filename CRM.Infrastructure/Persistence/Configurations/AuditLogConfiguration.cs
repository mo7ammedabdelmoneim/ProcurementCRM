using CRM.Domain.Entities.CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.EntityName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.EntityId).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Action).IsRequired().HasMaxLength(50);
        builder.Property(x => x.IpAddress).HasMaxLength(50);

        // OldValues / NewValues are JSON — no max length restriction
        builder.Property(x => x.OldValues).HasColumnType("nvarchar(max)");
        builder.Property(x => x.NewValues).HasColumnType("nvarchar(max)");

        // Append-only — never updated or soft-deleted
        builder.HasIndex(x => new { x.EntityName, x.EntityId });
        builder.HasIndex(x => x.CreatedAt);
    }
}