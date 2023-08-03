using EmployeeApp.Api.DTOs.Employee;

namespace EmployeeApp.Api.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IList<EmployeeDto>> GetEmployeesAsync();
        Task<EmployeeDto> GetEmployeeAsync(Guid employeeId);
        Task<bool> CreateAsync(CreateEmployeeDto employeeInput);
        Task<bool> UpdateAsync(Guid employeeId, UpdateEmployeeDto employeeInput);
        Task<bool> DeleteAsync(Guid employeeId);
    }
}