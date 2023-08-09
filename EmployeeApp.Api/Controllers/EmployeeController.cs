using System;
using EmployeeApp.Api.DTOs.Employee;
using EmployeeApp.Api.Entities;
using EmployeeApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
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

        [HttpPost("add")]
        public async Task<ActionResult> CreateNewEmployee(CreateEmployeeDto createEmployeeDto)
        {
            var employeeId = await _employeeService.CreateAsync(createEmployeeDto);
            if (employeeId == null)
                return BadRequest("An error occurred during creating");

            var newEmployee = await _employeeService.GetEmployeeAsync(employeeId.Value);

            return CreatedAtAction(nameof(GetEmployee), newEmployee, new { id = employeeId });

        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult<EmployeeDto>> UpdateComapny(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employeeId = await _employeeService.UpdateAsync(id, updateEmployeeDto);
            if (employeeId == null)
                return BadRequest("An error occurred during deleting");

            var updatedEmployee = await _employeeService.GetEmployeeAsync(employeeId.Value);

            return Ok(updatedEmployee);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            var result = await _employeeService.DeleteAsync(id);
            if (!result)
                return BadRequest("An error occurred during deleting");

            return Ok();
        }
    }
}