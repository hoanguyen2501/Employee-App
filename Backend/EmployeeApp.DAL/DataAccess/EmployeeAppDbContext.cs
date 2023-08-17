using EmployeeApp.DAL.DataAccess.Configurations;
using EmployeeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.DAL.DataAccess
{
    public class EmployeeAppDbContext : DbContext
    {
        public EmployeeAppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new AppUserConfiguration().Configure(modelBuilder.Entity<AppUser>());
            new CompanyConfiguration().Configure(modelBuilder.Entity<Company>());
            new EmployeeConfiguration().Configure(modelBuilder.Entity<Employee>());
        }
    }
}