using EmployeeApp.DAL.DataAccess;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeApp.DAL.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly EmployeeAppDbContext _context;

        public CompanyRepository(EmployeeAppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Company>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> GetCompanyById(Guid id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task<bool> Create(Company company)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task Update(Company company)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Companies.Update(company);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }

        public async Task Delete(Company company)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
