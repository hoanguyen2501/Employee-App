using EmployeeApp.Api.Controllers;
using EmployeeApp.DAL.DataAccess;
using EmployeeApp.DAL.DataAccess.Seeding;
using EmployeeApp.Service.Mapper;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace EmployeeApp.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://localhost:4200")
                            .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
                            .AllowCredentials()
                            .AllowAnyHeader();
                });
            });

            services.AddDbContext<EmployeeAppDbContext>(opts =>
            {
                string connectionString = config.GetConnectionString("EmployeeAppDatabase");
                opts.UseMySQL(connectionString);
            });

            services.AddAutoMapper(typeof(AutoMapperConfig));
            return services;
        }

        public static async Task<WebApplication> SeedingExtensionsAsync(this WebApplication app, Logger logger)
        {
            using IServiceScope scope = app.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;
            try
            {
                EmployeeAppDbContext context = services.GetRequiredService<EmployeeAppDbContext>();
                await context.Database.MigrateAsync();
                await Seed.SeedCompanies(context);
            }
            catch (Exception exception)
            {
                logger.Error(exception, "An error occurred during migration");
                throw;
            }

            return app;
        }

        public static IServiceCollection AddControlerAsServiceExtension(this IServiceCollection services)
        {
            services.AddTransient<RabbitMqController, RabbitMqController>();
            return services;
        }
    }
}