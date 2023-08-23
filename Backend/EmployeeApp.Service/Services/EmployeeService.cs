using AutoMapper;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.UOW;
using EmployeeApp.Service.DTOs.Employee;
using EmployeeApp.Service.Interfaces;
using EmployeeApp.Utils.Validation;
using Microsoft.Extensions.Logging;

namespace EmployeeApp.Service.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            IEnumerable<Employee> employees = await _unitOfWork.EmployeeRepository.QueryAllAsync();
            List<EmployeeDto> employeesToReturn = _mapper.Map<List<EmployeeDto>>(employees);

            return employeesToReturn;
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid id)
        {
            Employee employee = await _unitOfWork.EmployeeRepository.QueryByIdAsync(id);
            EmployeeDto employeeToReturn = _mapper.Map<EmployeeDto>(employee);

            return employeeToReturn;
        }

        public async Task<Guid?> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            try
            {
                Validation.TrimStringProperies(createEmployeeDto);
                Employee newEmployee = _mapper.Map<Employee>(createEmployeeDto);

                await _unitOfWork.EmployeeRepository.InsertAsync(newEmployee);
                await _unitOfWork.SaveAllAsync();

                return newEmployee.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Somethings went wrong during creating");
                return null;
            }
        }

        public async Task<EmployeeDto> UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                Validation.TrimStringProperies(updateEmployeeDto);
                Employee employee = await _unitOfWork.EmployeeRepository.QueryByIdAsync(id);
                _mapper.Map<UpdateEmployeeDto, Employee>(updateEmployeeDto, employee);

                await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                await _unitOfWork.SaveAllAsync();
                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Somethings went wrong during updating");
                return null;
            }
        }

        public async Task<bool> DeleteEmployee(Guid id)
        {
            try
            {
                var employee = _unitOfWork.EmployeeRepository.QueryByIdAsync(id);
                if (employee == null)
                    return false;

                await _unitOfWork.EmployeeRepository.DeleteAsync(id);
                await _unitOfWork.SaveAllAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Somethings went wrong during deleting");
                return false;
            }
        }

        public async Task<bool> CheckExistingEmployeeEmail(string email)
        {
            return await _unitOfWork.EmployeeRepository.QueryByConditionsAsync(cond => cond.Email == email) != null;
        }

        public async Task<bool> CheckExistingEmployeePhone(string phone)
        {
            return await _unitOfWork.EmployeeRepository.QueryByConditionsAsync(cond => cond.PhoneNumber == phone) != null;
        }
    }
}
