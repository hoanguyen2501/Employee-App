using EmployeeApp.DAL.DataAccess;
using EmployeeApp.Service.Mapper;
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

            services.AddAutoMapper(typeof(AutoMapperConfig));
            return services;
        }
    }
}