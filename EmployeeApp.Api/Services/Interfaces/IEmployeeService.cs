using EmployeeApp.Api.DTOs.Employee;

namespace EmployeeApp.Api.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IList<EmployeeDto>> GetEmployeesAsync();
        Task<EmployeeDto> GetEmployeeAsync(Guid employeeId);
        Task<Guid?> CreateAsync(CreateEmployeeDto employeeInput);
        Task<Guid?> UpdateAsync(Guid employeeId, UpdateEmployeeDto employeeInput);
        Task<bool> DeleteAsync(Guid employeeId);
    }
}