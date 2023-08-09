using EmployeeApp.Api.Repositories.Implementations;
using EmployeeApp.Api.Repositories.Interfaces;
using EmployeeApp.Api.Services.Implementations;
using EmployeeApp.Api.Services.Interfaces;

namespace EmployeeApp.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            return services;
        }
    }
}