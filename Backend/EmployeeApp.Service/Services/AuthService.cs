using EmployeeApp.Service.DTOs.AppUser;
using EmployeeApp.Service.Helpers;
using EmployeeApp.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EmployeeApp.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly KeycloakSettings _keycloakSettings;

        public AuthService(IConfiguration config, IOptions<KeycloakSettings> keycloakSettings)
        {
            _config = config;
            _keycloakSettings = keycloakSettings.Value;
        }

        public async Task<AppUserDto> Login(AuthRequest loginDto)
        {
            string authServerUrl = _keycloakSettings.AuthServerUrl;
            string realm = _keycloakSettings.Realm;
            string clientId = _keycloakSettings.Resource;
            string secret = _keycloakSettings.Secret;

            string requestUrl = authServerUrl + $"realms/{realm}/protocol/openid-connect/token";
            Dictionary<string, string> requestBody = new()
            {
                {"client_id", clientId},
                {"grant_type", "password"},
                {"client_secret", secret},
                {"username", loginDto.Username },
                {"password", loginDto.Password }
            };

            using HttpClient httpClient = new();
            using HttpRequestMessage request = new(HttpMethod.Post, requestUrl)
            {
                Content = new FormUrlEncodedContent(requestBody)
            };
            using HttpResponseMessage response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            string result = response.Content.ReadAsStringAsync().Result;
            Dictionary<string, string> resultDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            return new AppUserDto
            {
                Username = loginDto.Username,
                AccessToken = resultDic["access_token"],
                RefreshToken = resultDic["refresh_token"]
            };
        }

        public async Task<AppUserDto> Refresh(string username, string refreshToken)
        {
            string authServerUrl = _keycloakSettings.AuthServerUrl;
            string realm = _keycloakSettings.Realm;
            string clientId = _keycloakSettings.Resource;
            string secret = _keycloakSettings.Secret;

            string requestUrl = authServerUrl + $"realms/{realm}/protocol/openid-connect/token";
            Dictionary<string, string> requestBody = new()
            {
                {"client_id", clientId},
                {"grant_type", "refresh_token"},
                {"client_secret", secret},
                {"refresh_token", refreshToken},
            };

            using HttpClient httpClient = new();
            using HttpRequestMessage requestRefreshToken = new(HttpMethod.Post, requestUrl)
            {
                Content = new FormUrlEncodedContent(requestBody)
            };
            using HttpResponseMessage response = await httpClient.SendAsync(requestRefreshToken);

            if (!response.IsSuccessStatusCode)
                return null;

            string result = response.Content.ReadAsStringAsync().Result;
            Dictionary<string, string> resultDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            return new AppUserDto
            {
                Username = username,
                AccessToken = resultDic["access_token"],
                RefreshToken = refreshToken
            };
        }

        public async Task<bool> Logout(string refreshToken)
        {
            string authServerUrl = _keycloakSettings.AuthServerUrl;
            string realm = _keycloakSettings.Realm;
            string clientId = _keycloakSettings.Resource;
            string secret = _keycloakSettings.Secret;

            string requestUrl = authServerUrl + $"realms/{realm}/protocol/openid-connect/logout";
            Dictionary<string, string> requestBody = new()
            {
                {"client_id", clientId},
                {"client_secret", secret},
                {"refresh_token",refreshToken},
            };

            using HttpClient httpClient = new();
            using HttpRequestMessage requestRefreshToken = new(HttpMethod.Post, requestUrl)
            {
                Content = new FormUrlEncodedContent(requestBody)
            };
            using HttpResponseMessage response = await httpClient.SendAsync(requestRefreshToken);

            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
    }
}

