using EmployeeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeApp.DAL.DataAccess.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable(nameof(Company));
            builder.HasKey(pk => pk.Id);

            builder.HasIndex(i => i.Email)
                .IsUnique();

            builder.HasIndex(i => i.PhoneNumber)
                .IsUnique();

            builder.HasMany<Employee>(m => m.Employees)
                .WithOne(o => o.Company)
                .HasForeignKey(fk => fk.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}