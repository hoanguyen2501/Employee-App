using EmployeeApp.DAL.DataAccess;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.Repositories;

namespace EmployeeApp.DAL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeeAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
