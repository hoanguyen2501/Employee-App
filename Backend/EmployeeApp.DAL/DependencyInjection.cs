using EmployeeApp.DAL.UOW;
using EmployeeApp.Domain.UOW;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeApp.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositoryCollection(this IServiceCollection services)
        {
            // DI of Repository
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
