using EmployeeApp.Service.DTOs.Employee;

namespace EmployeeApp.Service.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetEmployeesAsync();
        Task<EmployeeDto> GetEmployeeByIdAsync(Guid id);
        Task<Guid?> CreateEmployee(CreateEmployeeDto createEmployeeDto);
        Task<EmployeeDto> UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto);
        Task<bool> DeleteEmployee(Guid id);
        Task<bool> CheckExistingEmployeeEmail(string email);
        Task<bool> CheckExistingEmployeePhone(string phone);
    }
}
