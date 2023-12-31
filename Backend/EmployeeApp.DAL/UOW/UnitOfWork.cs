﻿using EmployeeApp.DAL.DataAccess;
using EmployeeApp.DAL.Repositories;
using EmployeeApp.Domain.Repositories;
using EmployeeApp.Domain.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace EmployeeApp.DAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeAppDbContext _dbContext;
        public ICompanyRepository _companyRepository;
        public IEmployeeRepository _employeeRepository;
        private bool disposed = false;

        public UnitOfWork(EmployeeAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICompanyRepository CompanyRepository
        {
            get
            {
                _companyRepository ??= new CompanyRepository(_dbContext);
                return _companyRepository;
            }
        }

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                _employeeRepository ??= new EmployeeRepository(_dbContext);
                return _employeeRepository;
            }
        }

        public async Task<IDbContextTransaction> CreateTransaction()
        {
            return await _dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
        }

        public async Task Commit()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransaction()
        {
            await _dbContext.Database.RollbackTransactionAsync();
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
