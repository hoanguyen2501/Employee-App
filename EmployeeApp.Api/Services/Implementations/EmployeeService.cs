using AutoMapper;
using EmployeeApp.Api.DTOs.Employee;
using EmployeeApp.Api.Entities;
using EmployeeApp.Api.Repositories.Interfaces;
using EmployeeApp.Api.Services.Interfaces;

namespace EmployeeApp.Api.Services.Implementations
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

        public async Task<IList<EmployeeDto>> GetEmployeesAsync()
        {
            var employees = await _unitOfWork.Repository<Employee>().GetAllAsync(x => x.Company);

            var employeesToReturn = _mapper.Map<IList<EmployeeDto>>(employees);
            return employeesToReturn;
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid employeeId)
        {
            var employee = await _unitOfWork.Repository<Employee>().FindAsync(employeeId);

            var employeeToReturn = _mapper.Map<EmployeeDto>(employee);
            return employeeToReturn;
        }

        public async Task<bool> CreateAsync(CreateEmployeeDto employeeInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var employeeRepos = _unitOfWork.Repository<Employee>();
                var employee = _mapper.Map<Employee>(employeeInput);
                employee.Id = Guid.NewGuid();

                await employeeRepos.InsertAsync(employee);

                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Guid employeeId, UpdateEmployeeDto employeeInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var employeeRepos = _unitOfWork.Repository<Employee>();
                var employee = await employeeRepos.FindAsync(employeeId);
                if (employee == null)
                    return false;

                _mapper.Map(employeeInput, employee);
                await employeeRepos.UpdateAsync(employee);

                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid employeeId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var employeeRepos = _unitOfWork.Repository<Employee>();
                var employee = await employeeRepos.FindAsync(employeeId);
                if (employee == null)
                    return false;

                await employeeRepos.DeleteAsync(employeeId);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }
    }
}