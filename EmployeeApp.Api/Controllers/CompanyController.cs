using EmployeeApp.Api.DTOs.Company;
using EmployeeApp.Api.Entities;
using EmployeeApp.Api.Services.Implementations;
using EmployeeApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    public class CompanyController : BaseApiController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<CompanyDto>>> GetCompanies()
        {
            var companies = await _companyService.GetCompaniesAsync();
            if (companies == null)
                return NotFound();

            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyById(Guid id)
        {
            var company = await _companyService.GetCompanyAsync(id);
            if (company == null)
                return NotFound();

            return Ok(company);
        }

        [HttpPost("add")]
        public async Task<ActionResult> CreateNewCompany(CreateCompanyDto createCompanyDto)
        {
            var companyId = await _companyService.CreateAsync(createCompanyDto);
            if (companyId == null)
                return BadRequest("An error occurred during creating");

            var newCompany = await _companyService.GetCompanyAsync(companyId.Value);

            return CreatedAtAction(nameof(GetCompanyById), newCompany, new { id = companyId });
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult<CompanyDto>> UpdateComapny(Guid id, UpdateCompanyDto updateCompanyDto)
        {
            var companyId = await _companyService.UpdateAsync(id, updateCompanyDto);
            if (companyId == null)
                return BadRequest("An error occurred during deleting");

            var updatedCompany = await _companyService.GetCompanyAsync(companyId.Value);

            return Ok(updatedCompany);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCompany(Guid id)
        {
            var result = await _companyService.DeleteAsync(id);
            if (!result)
                return BadRequest("An error occurred during deleting");

            return Ok();
        }
    }
}