using EmployeeApp.Service.DTOs.Company;
using EmployeeApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    [Authorize]
    public class CompanyController : BaseApiController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompanyDto>>> GetCompanies()
        {
            var companies = await _companyService.GetCompaniesAsync();
            if (companies == null)
                return BadRequest();

            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyById(Guid id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
                return BadRequest();

            return Ok(company);
        }

        [HttpPost("add")]
        public async Task<ActionResult> CreateNewCompany(CreateCompanyDto createCompanyDto)
        {
            var isEmailExisted = await _companyService.CheckExistingCompanyEmail(createCompanyDto.Email);
            if (isEmailExisted)
                return BadRequest("Email has already taken");

            var isPhoneExisted = await _companyService.CheckExistingCompanyPhone(createCompanyDto.PhoneNumber);
            if (isPhoneExisted)
                return BadRequest("Phone number has already taken");

            var companyId = await _companyService.CreateCompany(createCompanyDto);
            return Ok(companyId);
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult<CompanyDto>> UpdateComapny(Guid id, UpdateCompanyDto updateCompanyDto)
        {
            var updatedCompany = await _companyService.UpdateCompany(id, updateCompanyDto);
            if (updatedCompany == null)
                return BadRequest("An error occurred during updating");

            return Ok(updatedCompany);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCompany(Guid id)
        {
            var result = await _companyService.DeleteCompany(id);
            if (!result)
                return BadRequest("An error occurred during deleting");

            return Ok();
        }
    }
}