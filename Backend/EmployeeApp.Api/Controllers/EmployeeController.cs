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
        private readonly RabbitMqController _rabbitMqController;

        public EmployeeController(IEmployeeService employeeService, RabbitMqController rabbitMqController)
        {
            _employeeService = employeeService;
            _rabbitMqController = rabbitMqController;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
        {
            List<EmployeeDto> employees = await _employeeService.GetEmployeesAsync();
            if (employees == null || employees.Count == 0)
                return NotFound();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(Guid id)
        {
            EmployeeDto employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost("add")]
        public async Task<ActionResult> CreateNewEmployee(CreateEmployeeDto createEmployeeDto)
        {
            bool isEmailExisted = await _employeeService.CheckExistingEmployeeEmail(createEmployeeDto.Email);
            if (isEmailExisted)
                return BadRequest("Email has already taken");

            bool isPhoneExisted = await _employeeService.CheckExistingEmployeePhone(createEmployeeDto.PhoneNumber);
            if (isPhoneExisted)
                return BadRequest("Phone number has already taken");

            Guid? employeeId = await _employeeService.CreateEmployee(createEmployeeDto);
            EmployeeDto newEmployee = await _employeeService.GetEmployeeByIdAsync(employeeId.Value);

            return CreatedAtAction(nameof(GetEmployeeById), newEmployee, new { id = employeeId });
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult<EmployeeDto>> UpdateComapny(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            EmployeeDto updatedEmployee = await _employeeService.UpdateEmployee(id, updateEmployeeDto);
            if (updatedEmployee == null)
                return BadRequest("An error occurred during updating");

            return Ok(updatedEmployee);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            bool result = await _employeeService.DeleteEmployee(id);
            if (!result)
                return BadRequest("An error occurred during deleting");

            return Ok();
        }
    }
}