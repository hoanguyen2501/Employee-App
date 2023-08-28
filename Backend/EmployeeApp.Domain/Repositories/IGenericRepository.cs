using System.Linq.Expressions;

namespace EmployeeApp.Domain.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> QueryAllAsync(params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> QueryByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> QueryAllByConditionsAsync(
            Expression<Func<TEntity, bool>> expression,
            params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> QueryByConditionsAsync(
            Expression<Func<TEntity, bool>> expression,
            params Expression<Func<TEntity, object>>[] includes);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    }
}
