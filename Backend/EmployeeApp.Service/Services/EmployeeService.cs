using AutoMapper;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.UOW;
using EmployeeApp.Service.DTOs.Employee;
using EmployeeApp.Service.Interfaces;
using EmployeeApp.Utils.Validation;

namespace EmployeeApp.Service.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            var employees = await _unitOfWork.EmployeeRepository.QueryAllAsync();
            var employeesToReturn = _mapper.Map<List<EmployeeDto>>(employees);

            return employeesToReturn;
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _unitOfWork.EmployeeRepository.QueryByIdAsync(id);
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

                await _unitOfWork.EmployeeRepository.InsertAsync(newEmployee);
                await _unitOfWork.SaveAllAsync();

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
                var employee = await _unitOfWork.EmployeeRepository.QueryByIdAsync(id);
                _mapper.Map<UpdateEmployeeDto, Employee>(updateEmployeeDto, employee);

                await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                await _unitOfWork.SaveAllAsync();
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
                await _unitOfWork.EmployeeRepository.DeleteAsync(id);
                await _unitOfWork.SaveAllAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
