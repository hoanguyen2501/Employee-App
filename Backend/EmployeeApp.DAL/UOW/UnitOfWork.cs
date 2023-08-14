using EmployeeApp.DAL.DataAccess;
using EmployeeApp.DAL.Repositories;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.Repositories;
using EmployeeApp.Domain.UOW;
using System;
using System.Threading.Tasks;

namespace EmployeeApp.DAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeAppDbContext _dbContext;
        public IGenericRepository<Company> _companyRepository;
        public IGenericRepository<Employee> _employeeRepository;
        private bool disposed = false;

        public UnitOfWork(EmployeeAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<Company> CompanyRepository
        {
            get
            {
                _companyRepository ??= new GenericRepository<Company>(_dbContext);
                return _companyRepository;
            }
        }

        public IGenericRepository<Employee> EmployeeRepository
        {
            get
            {
                _employeeRepository ??= new GenericRepository<Employee>(_dbContext);
                return _employeeRepository;
            }
        }

        public async Task SaveAllAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                    _employeeRepository = null;
                    _companyRepository = null;
                }
            }
            this.disposed = true;
        }
    }
}
