using EmployeeApp.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Api.Repositories
{
    public class DbFactory : IDisposable
    {
        private bool _disposed;
        private Func<EmployeeAppDbContext> _instanceFunc;
        private DbContext _dbContext;

        public DbFactory(Func<EmployeeAppDbContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}