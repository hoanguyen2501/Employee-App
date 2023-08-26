﻿using EmployeeApp.Service.DTOs.AppUser;
using EmployeeApp.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EmployeeApp.Service.Services
{
    public class AuthService : IAutheService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<AppUserDto> Login(AppUserLoginDto loginDto)
        {
            string realm = _config["Keycloak:realm"];
            string clientId = _config["Keycloak:resource"];
            string secret = _config["Keycloak:credentials:secret"];
            string authServerUrl = $"{_config["Keycloak:auth-server-url"]}";

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

        public async Task<AppUserDto> Refresh(AppUserRefreshDto refreshUserDto)
        {
            string realm = _config["Keycloak:realm"];
            string clientId = _config["Keycloak:resource"];
            string secret = _config["Keycloak:credentials:secret"];
            string authServerUrl = $"{_config["Keycloak:auth-server-url"]}";

            string requestUrl = authServerUrl + $"realms/{realm}/protocol/openid-connect/token";
            Dictionary<string, string> requestBody = new()
            {
                {"client_id", clientId},
                {"grant_type", "refresh_token"},
                {"client_secret", secret},
                {"refresh_token", refreshUserDto.RefreshToken},
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
                Username = refreshUserDto.Username,
                AccessToken = resultDic["access_token"],
                RefreshToken = refreshUserDto.RefreshToken
            };
        }

        public async Task<bool?> GetUserByUsername(string username, string accessToken)
        {
            //string realm = _config["Keycloak:realm"];
            //string authServerUrl = $"{_config["Keycloak:auth-server-url"]}";

            //string requestUrl = authServerUrl + $"admin/realms/{realm}/users?username={username}&exact=true";

            //using HttpClient httpClient = new();
            //using HttpRequestMessage requestRefreshToken = new(HttpMethod.Post, requestUrl);
            //requestRefreshToken.Headers.Add("Authorization", $"Bearer {accessToken}");
            //requestRefreshToken.Headers.Remove("Content-Type");

            //using HttpResponseMessage response = await httpClient.SendAsync(requestRefreshToken);

            //if (!response.IsSuccessStatusCode)
            //    return null;

            //string result = response.Content.ReadAsStringAsync().Result;
            //List<object> user = JsonConvert.DeserializeObject<List<object>>(result);

            //return user.Count > 0;
            throw new NotImplementedException();
        }
    }
}
