using AutoMapper;
using EmployeeApp.Api.DTOs.Company;
using EmployeeApp.Api.Entities;
using EmployeeApp.Api.Repositories.Interfaces;
using EmployeeApp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Api.Services.Implementations
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

        public async Task<IList<CompanyDto>> GetCompaniesAsync()
        {
            var companies = await _unitOfWork.Repository<Company>().GetAllAsync();

            var companiesToReturn = _mapper.Map<IList<CompanyDto>>(companies);
            return companiesToReturn;
        }

        public async Task<CompanyDto> GetCompanyAsync(Guid companyId)
        {
            var company = await _unitOfWork.Repository<Company>().FindAsync(companyId);

            var companyToReturn = _mapper.Map<CompanyDto>(company);
            return companyToReturn;
        }

        public async Task<Guid?> CreateAsync(CreateCompanyDto companyInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var companyRepos = _unitOfWork.Repository<Company>();
                var company = _mapper.Map<Company>(companyInput);
                company.Id = Guid.NewGuid();

                await companyRepos.InsertAsync(company);

                await _unitOfWork.CommitTransaction();
                return company.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                return null;
            }
        }

        public async Task<Guid?> UpdateAsync(Guid companyId, UpdateCompanyDto companyInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var companyRepos = _unitOfWork.Repository<Company>();
                var company = await companyRepos.FindAsync(companyId);
                if (company == null)
                    return null;

                _mapper.Map<UpdateCompanyDto, Company>(companyInput, company);
                await companyRepos.UpdateAsync(company);

                await _unitOfWork.CommitTransaction();
                return company.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid companyId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var companyRepos = _unitOfWork.Repository<Company>();
                var company = await companyRepos.FindAsync(companyId);
                if (company == null)
                    return false;

                await companyRepos.DeleteAsync(companyId);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }
    }
}