using System.Linq.Expressions;
using EmployeeApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Api.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbContext DbContext { get; private set; }
        public DbSet<T> Entities => DbContext.Set<T>();
        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public T Find(params object[] keyValues)
        {
            return Entities.Find(keyValues);
        }

        public async Task<T> FindAsync(params object[] keyValues)
        {
            return await Entities.FindAsync(keyValues);
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<IList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = Entities;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task InsertAsync(T entity, bool saveChanges = true)
        {
            await Entities.AddAsync(entity);
            if (saveChanges)
                await DbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity, bool saveChanges = true)
        {
            Entities.Update(entity);
            if (saveChanges)
                await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, bool saveChanges = true)
        {
            var entity = await Entities.FindAsync(id);
            Entities.Remove(entity);
            if (saveChanges)
                await DbContext.SaveChangesAsync();
        }
    }
}