using EmployeeApp.Service.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace EmployeeApp.Api.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly KeycloakSettings _keycloakSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<KeycloakSettings> keycloakSettings)
        {
            _next = next;
            _keycloakSettings = keycloakSettings.Value;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            bool isAllowedAnonymous = httpContext.GetEndpoint().Metadata
                                                .GetMetadata<AllowAnonymousAttribute>() is not null;

            if (isAllowedAnonymous)
                await _next(httpContext);
            else
            {
                string accessToken = httpContext.Request.Headers["Authorization"]
                                        .FirstOrDefault()?.Split(" ").Last();
                if (accessToken == null)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
                else if (httpContext.Request.Path.Value.Split("/").Last().ToLower() == "refresh")
                {
                    await _next(httpContext);
                }
                else
                {
                    string authServerUrl = _keycloakSettings.AuthServerUrl;
                    string realm = _keycloakSettings.Realm;
                    string resource = _keycloakSettings.Resource;
                    string secret = _keycloakSettings.Secret;

                    string requestUrl = authServerUrl + $"realms/{realm}/protocol/openid-connect/token/introspect";
                    Dictionary<string, string> requestBody = new()
                    {
                        {"client_id", resource},
                        {"client_secret", secret},
                        {"token", accessToken},
                    };

                    using HttpClient httpClient = new();
                    using HttpRequestMessage request = new(HttpMethod.Post, requestUrl)
                    {
                        Content = new FormUrlEncodedContent(requestBody)
                    };
                    using HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return;
                    }

                    dynamic result = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);

                    if (result["active"] == false)
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return;
                    }

                    await _next(httpContext);
                }
            }
        }
    }
}
