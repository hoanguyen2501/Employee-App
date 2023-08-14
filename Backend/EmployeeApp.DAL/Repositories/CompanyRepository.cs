using EmployeeApp.DAL.DataAccess;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.Repositories;

namespace EmployeeApp.DAL.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(EmployeeAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
