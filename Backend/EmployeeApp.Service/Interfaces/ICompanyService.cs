using EmployeeApp.Service.DTOs.Company;

namespace EmployeeApp.Service.Interfaces
{
    public interface ICompanyService
    {
        Task<List<CompanyDto>> GetCompaniesAsync();
        Task<CompanyDto> GetCompanyByIdAsync(Guid id);
        Task<Guid?> CreateCompany(CreateCompanyDto createCompanyDto);
        Task<CompanyDto> UpdateCompany(Guid id, UpdateCompanyDto updateCompanyDto);
        Task<bool> DeleteCompany(Guid id);
    }
}
