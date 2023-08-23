using AutoMapper;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.UOW;
using EmployeeApp.Service.DTOs.Company;
using EmployeeApp.Service.Interfaces;
using EmployeeApp.Utils.Validation;
using Microsoft.Extensions.Logging;

namespace EmployeeApp.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CompanyService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CompanyDto>> GetCompaniesAsync()
        {
            IEnumerable<Company> companies = await _unitOfWork.CompanyRepository.QueryAllAsync();
            List<CompanyDto> companiesToReturn = _mapper.Map<List<CompanyDto>>(companies);

            return companiesToReturn;
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(Guid id)
        {
            Company company = await _unitOfWork.CompanyRepository.QueryByIdAsync(id);
            CompanyDto companyToReturn = _mapper.Map<CompanyDto>(company);

            return companyToReturn;
        }

        public async Task<Guid?> CreateCompany(CreateCompanyDto createCompanyDto)
        {
            try
            {
                Validation.TrimStringProperies(createCompanyDto);
                Company newCompany = _mapper.Map<Company>(createCompanyDto);
                newCompany.Id = Guid.NewGuid();

                await _unitOfWork.CompanyRepository.InsertAsync(newCompany);
                await _unitOfWork.SaveAllAsync();
                return newCompany.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Somethings went wrong during creating");
                return null;
            }
        }

        public async Task<CompanyDto> UpdateCompany(Guid id, UpdateCompanyDto updateCompanyDto)
        {
            try
            {
                Validation.TrimStringProperies(updateCompanyDto);
                Company company = await _unitOfWork.CompanyRepository.QueryByIdAsync(id);
                _mapper.Map<UpdateCompanyDto, Company>(updateCompanyDto, company);

                await _unitOfWork.CompanyRepository.UpdateAsync(company);
                await _unitOfWork.SaveAllAsync();
                return _mapper.Map<CompanyDto>(company);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Somethings went wrong during updating");
                return null;
            }
        }

        public async Task<bool> DeleteCompany(Guid id)
        {
            try
            {
                await _unitOfWork.CompanyRepository.DeleteAsync(id);
                await _unitOfWork.SaveAllAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Somethings went wrong during deleting");
                return false;
            }
        }

        public async Task<bool> CheckExistingCompanyEmail(string email)
        {
            return await _unitOfWork.CompanyRepository.QueryByConditionsAsync(cond => cond.Email == email) != null;
        }

        public async Task<bool> CheckExistingCompanyPhone(string phone)
        {
            return await _unitOfWork.CompanyRepository.QueryByConditionsAsync(cond => cond.PhoneNumber == phone) != null;
        }
    }
}
