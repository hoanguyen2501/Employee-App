using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.Repositories;

namespace EmployeeApp.Domain.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Company> CompanyRepository { get; }
        IGenericRepository<Employee> EmployeeRepository { get; }
        Task SaveAllAsync();
    }
}
