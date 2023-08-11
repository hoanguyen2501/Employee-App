using EmployeeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeApp.DAL.DataAccess.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee));
            builder.HasKey(pk => pk.Id);

            builder.HasIndex(i => i.Email)
                .IsUnique();

            builder.HasIndex(i => i.PhoneNumber)
                .IsUnique();

            builder.Property(p => p.CompanyId)
                .IsRequired(false);
        }
    }
}