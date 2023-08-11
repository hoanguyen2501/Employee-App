using AutoMapper;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.Repositories;
using EmployeeApp.Service.DTOs.Company;
using EmployeeApp.Service.Interfaces;
using EmployeeApp.Utils.Validation;

namespace EmployeeApp.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<List<CompanyDto>> GetCompaniesAsync()
        {
            var companies = await _companyRepository.GetCompanies();
            var companiesToReturn = _mapper.Map<List<CompanyDto>>(companies);

            return companiesToReturn;
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(Guid id)
        {
            var company = await _companyRepository.GetCompanyById(id);
            var companyToReturn = _mapper.Map<CompanyDto>(company);

            return companyToReturn;
        }

        public async Task<Guid?> CreateCompany(CreateCompanyDto createCompanyDto)
        {
            try
            {
                Validation.TrimStringProperies(createCompanyDto);
                var newCompany = _mapper.Map<Company>(createCompanyDto);
                newCompany.Id = Guid.NewGuid();

                var isCreated = await _companyRepository.Create(newCompany);
                if (!isCreated)
                    return null;

                return newCompany.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CompanyDto> UpdateCompany(Guid id, UpdateCompanyDto updateCompanyDto)
        {
            try
            {
                Validation.TrimStringProperies(updateCompanyDto);
                var company = await _companyRepository.GetCompanyById(id);
                _mapper.Map<UpdateCompanyDto, Company>(updateCompanyDto, company);

                await _companyRepository.Update(company);
                return _mapper.Map<CompanyDto>(company);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteCompany(Guid id)
        {
            try
            {
                var company = await _companyRepository.GetCompanyById(id);
                var updatedCompany = _mapper.Map<Company>(company);

                await _companyRepository.Delete(updatedCompany);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
