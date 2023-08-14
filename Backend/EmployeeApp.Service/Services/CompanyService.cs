using AutoMapper;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.UOW;
using EmployeeApp.Service.DTOs.Company;
using EmployeeApp.Service.Interfaces;
using EmployeeApp.Utils.Validation;

namespace EmployeeApp.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CompanyDto>> GetCompaniesAsync()
        {
            var companies = await _unitOfWork.CompanyRepository.QueryAllAsync();
            var companiesToReturn = _mapper.Map<List<CompanyDto>>(companies);

            return companiesToReturn;
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(Guid id)
        {
            var company = await _unitOfWork.CompanyRepository.QueryByIdAsync(id);
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

                await _unitOfWork.CompanyRepository.InsertAsync(newCompany);
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
                var company = await _unitOfWork.CompanyRepository.QueryByIdAsync(id);
                _mapper.Map<UpdateCompanyDto, Company>(updateCompanyDto, company);

                await _unitOfWork.CompanyRepository.UpdateAsync(company);
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
                await _unitOfWork.CompanyRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
