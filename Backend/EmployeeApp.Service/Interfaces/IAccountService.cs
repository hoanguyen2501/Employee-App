using EmployeeApp.Service.DTOs.AppUser;

namespace EmployeeApp.Service.Interfaces
{
    public interface IAccountService
    {
        Task<AppUserDto> Login(AppUserLoginDto loginDto);
    }
}
