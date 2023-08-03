using EmployeeApp.Api.Data;
using EmployeeApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<EmployeeAppDbContext>(opts =>
            {
                string connectionString = config.GetConnectionString("EmployeeAppDatabase");
                opts.UseMySQL(connectionString);
            });

            services.AddScoped<Func<EmployeeAppDbContext>>((provider) => () =>
                provider.GetService<EmployeeAppDbContext>());

            services.AddScoped<DbFactory>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}