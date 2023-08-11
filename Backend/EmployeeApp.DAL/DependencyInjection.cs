using EmployeeApp.DAL.Repositories;
using EmployeeApp.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeApp.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositoryCollection(this IServiceCollection services)
        {
            // DI of Repository
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;
        }
    }
}
