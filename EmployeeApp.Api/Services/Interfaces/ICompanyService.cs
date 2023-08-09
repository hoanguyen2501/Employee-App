using EmployeeApp.Api.DTOs.Company;

namespace EmployeeApp.Api.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IList<CompanyDto>> GetCompaniesAsync();
        Task<CompanyDto> GetCompanyAsync(Guid companyId);
        Task<Guid?> CreateAsync(CreateCompanyDto companyInput);
        Task<Guid?> UpdateAsync(Guid companyId, UpdateCompanyDto companyInput);
        Task<bool> DeleteAsync(Guid companyId);

    }
}