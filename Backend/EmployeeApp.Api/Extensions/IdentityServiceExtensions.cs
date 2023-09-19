using EmployeeApp.Service.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeApp.Api.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddJwtIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KeycloakSettings>(configuration.GetSection("Keycloak"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                string keycloakServerUrl = configuration["Keycloak:AuthServerUrl"] + $"realms/{configuration["Keycloak:Realm"]}/";
                string clientId = configuration["Keycloak:Resource"];
                options.Authority = keycloakServerUrl;
                options.Audience = clientId;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = keycloakServerUrl,
                    ValidAudiences = new List<string> { "master-realm", "account", clientId },
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
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
