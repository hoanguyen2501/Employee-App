using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Api.Data;
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

            return services;
        }
    }
}