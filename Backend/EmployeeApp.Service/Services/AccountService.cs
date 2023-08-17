using EmployeeApp.Domain.UOW;
using EmployeeApp.Service.DTOs.AppUser;
using EmployeeApp.Service.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeApp.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public AccountService(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<AppUserDto> Login(AppUserLoginDto loginDto)
        {
            var user = await _unitOfWork.AccountRepository.QueryByConditionsAsync(x => x.Username == loginDto.Username);
            if (user == null)
                return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            var computedHashLength = computedHash.Length;
            for (int i = 0; i < computedHashLength; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return null;
            }

            return new AppUserDto
            {
                Username = user.Username,
                Token = _jwtService.CreateJwt(user)
            };
        }
    }
}
