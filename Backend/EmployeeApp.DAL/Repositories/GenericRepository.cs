using EmployeeApp.DAL.DataAccess;
using EmployeeApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeApp.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly EmployeeAppDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(EmployeeAppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> QueryAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
                return await _dbSet.ToListAsync<TEntity>();

            return await _dbSet.Include(includes => includes).ToListAsync();
        }

        public async Task<TEntity> QueryByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
                return await _dbSet.FindAsync(id);

            return await _dbSet.Include(includes => includes).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> QueryAllByConditionsAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
                return await _dbSet.Where(expression).ToListAsync();

            return await _dbSet.Where(expression).Include(includes => includes).ToListAsync();
        }

        public async Task<TEntity> QueryByConditionsAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
                return await _dbSet.Where(expression).FirstOrDefaultAsync();

            return await _dbSet.Where(expression).Include(includes => includes).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistedValue(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null) return false;

            return await _dbSet.Where(expression).FirstOrDefaultAsync() != null;
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            await Task.CompletedTask;
        }

        public Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }


        public Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
