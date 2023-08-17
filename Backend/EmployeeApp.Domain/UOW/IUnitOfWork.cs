using EmployeeApp.Domain.Repositories;

namespace EmployeeApp.Domain.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository CompanyRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IAccountRepository AccountRepository { get; }

        Task SaveAllAsync();
    }
}
