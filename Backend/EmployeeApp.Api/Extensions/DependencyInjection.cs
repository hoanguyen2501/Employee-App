using EmployeeApp.Service;

namespace EmployeeApp.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddServiceCollection();

            return services;
        }
    }
}
