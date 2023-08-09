using System.Security.Cryptography;
using System.Text;
using EmployeeApp.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeApp.Api.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable(nameof(AppUser));
            builder.HasKey(pk => pk.Id);

            builder.HasIndex(i => i.Username)
                .IsUnique();

            using var hmac = new HMACSHA512();
            builder.HasData(new AppUser
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("12345678")),
                PasswordSalt = hmac.Key
            });
        }
    }
}