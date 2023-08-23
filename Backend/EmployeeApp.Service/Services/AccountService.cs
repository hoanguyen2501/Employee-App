using EmployeeApp.Domain.UOW;
using EmployeeApp.Service.DTOs.AppUser;
using EmployeeApp.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EmployeeApp.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _config;

        public AccountService(IUnitOfWork unitOfWork, IJwtService jwtService, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
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

            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new FormUrlEncodedContent(requestBody)
            };
            using var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = response.Content.ReadAsStringAsync().Result;
            var resultDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            return new AppUserDto
            {
                Username = loginDto.Username,
                Token = resultDic["access_token"]
            };
        }
        //var user = await _unitOfWork.AccountRepository.QueryByConditionsAsync(x => x.Username == loginDto.Username);
        //if (user == null)
        //    return null;

        //using var hmac = new HMACSHA512(user.PasswordSalt);
        //var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        //var computedHashLength = computedHash.Length;
        //for (int i = 0; i < computedHashLength; i++)
        //{
        //    if (computedHash[i] != user.PasswordHash[i])
        //        return null;
        //}

        //return new AppUserDto
        //{
        //    Username = user.Username,
        //    Token = _jwtService.CreateJwt(user)
        //};
    }
}

