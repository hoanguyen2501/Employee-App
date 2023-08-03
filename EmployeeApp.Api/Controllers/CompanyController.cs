using EmployeeApp.Api.DTOs.Company;
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
        public async Task<ActionResult<CompanyDto>> GetCompanyById(string id)
        {
            var company = await _companyService.GetCompanyAsync(Guid.Parse(id));
            if (company == null)
                return NotFound();

            return Ok(company);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewCompany(CreateCompanyDto createCompanyDto)
        {
            var result = await _companyService.CreateAsync(createCompanyDto);
            if (!result)
                return BadRequest("An error occurred during creating");

            return NoContent();

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateComapny(Guid id, UpdateCompanyDto updateCompanyDto)
        {
            var result = await _companyService.UpdateAsync(id, updateCompanyDto);
            if (!result)
                return BadRequest("An error occurred during deleting");

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompany(Guid id)
        {
            var result = await _companyService.DeleteAsync(id);
            if (!result)
                return BadRequest("An error occurred during deleting");

            return Ok();
        }
    }
}