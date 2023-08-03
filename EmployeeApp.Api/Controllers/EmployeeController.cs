using EmployeeApp.Api.DTOs.Employee;
using EmployeeApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            if (employees == null)
                return NotFound();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewEmployee(CreateEmployeeDto createEmployeeDto)
        {
            var result = await _employeeService.CreateAsync(createEmployeeDto);
            if (!result)
                return BadRequest("An error occurred during creating");

            return NoContent();

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateComapny(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var result = await _employeeService.UpdateAsync(id, updateEmployeeDto);
            if (!result)
                return BadRequest("An error occurred during deleting");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            var result = await _employeeService.DeleteAsync(id);
            if (!result)
                return BadRequest("An error occurred during deleting");

            return Ok();
        }
    }
}