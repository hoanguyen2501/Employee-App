using EmployeeApp.DAL;
using EmployeeApp.Service.Interfaces;
using EmployeeApp.Service.RabbitMQ;
using EmployeeApp.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeApp.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {
            // DI of Repository
            services.AddRepositoryCollection();

            // DI of Services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            // DI of RabbitMQ Sender
            services.AddScoped<IMessageSender, MessageSender>();

            return services;
        }
    }
}
