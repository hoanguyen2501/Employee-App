using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Api.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Entities { get; }
        DbContext DbContext { get; }
        Task<IList<T>> GetAllAsync();
        Task<IList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        T Find(params object[] keyValues);
        Task<T> FindAsync(params object[] keyValues);
        Task InsertAsync(T entity, bool saveChanges = true);
        Task UpdateAsync(T entity, bool saveChanges = true);
        Task DeleteAsync(Guid id, bool saveChanges = true);
    }
}