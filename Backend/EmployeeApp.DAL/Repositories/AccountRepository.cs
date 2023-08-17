using EmployeeApp.DAL.DataAccess;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmployeeApp.DAL.Repositories
{
    public class AccountRepository : GenericRepository<AppUser>, IAccountRepository
    {
        private readonly EmployeeAppDbContext _dbContext;
        public AccountRepository(EmployeeAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsUserExistedAsync(string username)
        {
            return await _dbContext.AppUsers.AnyAsync(x => x.Username == username);
        }
    }
}
