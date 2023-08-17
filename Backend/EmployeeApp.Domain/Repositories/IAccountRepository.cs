using EmployeeApp.Domain.Entities;

namespace EmployeeApp.Domain.Repositories
{
    public interface IAccountRepository : IGenericRepository<AppUser>
    {
        Task<bool> IsUserExistedAsync(string username);
    }
}
