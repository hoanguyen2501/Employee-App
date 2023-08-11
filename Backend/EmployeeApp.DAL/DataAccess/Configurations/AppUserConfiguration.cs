using EmployeeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeApp.DAL.DataAccess.Configurations
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