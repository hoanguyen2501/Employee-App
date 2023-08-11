using EmployeeApp.DAL.DataAccess;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeApp.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeAppDbContext _context;

        public EmployeeRepository(EmployeeAppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Employee>> GetEmployees()
        {
            return await _context.Employees.Include(i => i.Company).ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(Guid id)
        {
            return await _context.Employees.Include(i => i.Company)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Create(Employee employee)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Employees.Add(employee);
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

        public async Task Update(Employee employee)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }

        public async Task Delete(Employee employee)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Employees.Remove(employee);
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
