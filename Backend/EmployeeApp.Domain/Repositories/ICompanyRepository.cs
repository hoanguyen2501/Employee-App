using EmployeeApp.Domain.Entities;

namespace EmployeeApp.Domain.Repositories
{
    public interface ICompanyRepository
    {
        Task<IList<Company>> GetCompanies();
        Task<Company> GetCompanyById(Guid id);
        Task<bool> Create(Company company);
        Task Update(Company company);
        Task Delete(Company company);
    }
}
