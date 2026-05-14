using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Infrastructure.Persistence.Configurations;
public class LeadConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(320);
        builder.Property(x => x.Phone).HasMaxLength(30);
        builder.Property(x => x.Notes).HasMaxLength(2000);

        // Store enum as string 
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.Source).HasConversion<string>().HasMaxLength(50);

        // Composite index 
        builder.HasIndex(x => new { x.Status, x.AssignedToUserId });
        builder.HasIndex(x => x.Email);

        // Global soft-delete filter 
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}