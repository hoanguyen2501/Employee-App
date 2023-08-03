using EmployeeApp.Api.DTOs.Company;
using EmployeeApp.Api.Entities;

namespace EmployeeApp.Api.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IList<CompanyDto>> GetCompaniesAsync();
        Task<CompanyDto> GetCompanyAsync(Guid companyId);
        Task<bool> CreateAsync(CreateCompanyDto companyInput);
        Task<bool> UpdateAsync(Guid companyId, UpdateCompanyDto companyInput);
        Task<bool> DeleteAsync(Guid companyId);

    }
}