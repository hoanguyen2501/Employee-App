using AutoMapper;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.Repositories;
using EmployeeApp.Service.DTOs.Employee;
using EmployeeApp.Service.Interfaces;
using EmployeeApp.Utils.Validation;

namespace EmployeeApp.Service.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetEmployees();
            var employeesToReturn = _mapper.Map<List<EmployeeDto>>(employees);

            return employeesToReturn;
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);
            var employeeToReturn = _mapper.Map<EmployeeDto>(employee);

            return employeeToReturn;
        }

        public async Task<Guid?> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            try
            {
                Validation.TrimStringProperies(createEmployeeDto);
                var newEmployee = _mapper.Map<Employee>(createEmployeeDto);
                newEmployee.Id = Guid.NewGuid();

                var isCreated = await _employeeRepository.Create(newEmployee);
                if (!isCreated)
                    return null;

                return newEmployee.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EmployeeDto> UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                Validation.TrimStringProperies(updateEmployeeDto);
                var employee = await _employeeRepository.GetEmployeeById(id);
                _mapper.Map<UpdateEmployeeDto, Employee>(updateEmployeeDto, employee);

                await _employeeRepository.Update(employee);
                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteEmployee(Guid id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeById(id);
                var updatedEmployee = _mapper.Map<Employee>(employee);

                await _employeeRepository.Delete(updatedEmployee);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
