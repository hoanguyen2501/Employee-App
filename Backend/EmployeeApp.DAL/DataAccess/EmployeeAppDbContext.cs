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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new CompanyConfiguration().Configure(modelBuilder.Entity<Company>());
            new EmployeeConfiguration().Configure(modelBuilder.Entity<Employee>());
        }
    }
}