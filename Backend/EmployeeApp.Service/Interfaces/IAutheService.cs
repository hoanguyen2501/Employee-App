using EmployeeApp.Service.DTOs.AppUser;

namespace EmployeeApp.Service.Interfaces
{
    public interface IAutheService
    {
        Task<AppUserDto> Login(AppUserLoginDto loginDto);
        Task<AppUserDto> Refresh(AppUserRefreshDto refreshUserDto);
        Task<bool?> GetUserByUsername(string username, string accessToken);
    }
}
