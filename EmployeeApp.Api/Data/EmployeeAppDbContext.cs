using EmployeeApp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Api.Data
{
    public class EmployeeAppDbContext : DbContext
    {
        public EmployeeAppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }


    }
}