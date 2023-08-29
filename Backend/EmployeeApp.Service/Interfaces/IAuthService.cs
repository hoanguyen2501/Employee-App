using EmployeeApp.Service.DTOs.AppUser;

namespace EmployeeApp.Service.Interfaces
{
    public interface IAuthService
    {
        Task<AppUserDto> Login(AuthRequest loginDto);
        Task<AppUserDto> Refresh(string username, string refreshToken);
        Task<bool> Logout(string refreshToken);
    }
}
