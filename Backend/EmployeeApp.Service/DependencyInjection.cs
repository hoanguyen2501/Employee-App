﻿using EmployeeApp.DAL;
using EmployeeApp.Service.Interfaces;
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
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            return services;
        }
    }
}
