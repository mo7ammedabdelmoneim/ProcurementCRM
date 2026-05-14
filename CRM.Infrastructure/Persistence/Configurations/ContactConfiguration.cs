using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Infrastructure.Persistence.Configurations;
public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(320);
        builder.Property(x => x.Phone).HasMaxLength(30);
        builder.Property(x => x.Company).HasMaxLength(200);
        builder.Property(x => x.JobTitle).HasMaxLength(150);
        builder.Property(x => x.Notes).HasMaxLength(2000);

        // Ignore computed property 
        builder.Ignore(x => x.FullName);

        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}