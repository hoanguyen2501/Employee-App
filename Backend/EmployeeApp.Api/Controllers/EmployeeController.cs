using EmployeeApp.Service.DTOs.Employee;
using EmployeeApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    [Authorize]
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
        {
            var companies = await _employeeService.GetEmployeesAsync();
            if (companies == null)
                return BadRequest();
            else if (companies.Count == 0)
                return NotFound();

            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(Guid id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost("add")]
        public async Task<ActionResult> CreateNewEmployee(CreateEmployeeDto createEmployeeDto)
        {
            var isEmailExisted = await _employeeService.CheckExistingEmployeeEmail(createEmployeeDto.Email);
            if (isEmailExisted)
                return BadRequest("Email has already taken");

            var isPhoneExisted = await _employeeService.CheckExistingEmployeePhone(createEmployeeDto.PhoneNumber);
            if (isPhoneExisted)
                return BadRequest("Phone number has already taken");

            var employeeId = await _employeeService.CreateEmployee(createEmployeeDto);
            var newEmployee = await _employeeService.GetEmployeeByIdAsync(employeeId.Value);
            return CreatedAtAction(nameof(GetEmployeeById), newEmployee, new { id = employeeId });
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult<EmployeeDto>> UpdateComapny(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var updatedEmployee = await _employeeService.UpdateEmployee(id, updateEmployeeDto);
            if (updatedEmployee == null)
                return BadRequest("An error occurred during updating");

            return Ok(updatedEmployee);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            var result = await _employeeService.DeleteEmployee(id);
            if (!result)
                return BadRequest("An error occurred during deleting");

            return Ok();
        }
    }
}