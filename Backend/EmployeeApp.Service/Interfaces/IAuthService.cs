using EmployeeApp.Service.DTOs.AppUser;

namespace EmployeeApp.Service.Interfaces
{
    public interface IAuthService
    {
        Task<AppUserDto> Login(AppUserLoginDto loginDto);
        Task<AppUserDto> Refresh(AppUserRefreshDto refreshUserDto);
        Task<bool> Logout(string refreshToken);
    }
}
