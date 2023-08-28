using EmployeeApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeApp.Domain.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository CompanyRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }

        Task<IDbContextTransaction> CreateTransaction();

        Task SaveAllAsync();
    }
}
