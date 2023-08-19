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
                await _unitOfWork.SaveAllAsync();
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
                await _unitOfWork.SaveAllAsync();
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
                await _unitOfWork.SaveAllAsync();
                return true;
            }
            catch (Exception)
            {
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
