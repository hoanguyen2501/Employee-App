using EmployeeApp.Domain.Entities;

namespace EmployeeApp.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IList<Employee>> GetEmployees();
        Task<Employee> GetEmployeeById(Guid id);
        Task<bool> Create(Employee employee);
        Task Update(Employee employee);
        Task Delete(Employee employee);
    }
}
