using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeApp.Api.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddJwtIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                string keycloakServerUrl = configuration["Keycloak:auth-server-url"] + $"realms/{configuration["Keycloak:realm"]}/";
                string clientId = configuration["Keycloak:resource"];
                options.Authority = keycloakServerUrl;
                options.Audience = clientId;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = keycloakServerUrl,
                    ValidAudiences = new List<string> { "master-realm", "account", clientId },
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Validate();
            });

            services.AddAuthorization();

            return services;
        }
    }
}
