using EmployeeApp.Domain.Entities;

namespace EmployeeApp.Service.Interfaces
{
    public interface IJwtService
    {
        string CreateJwt(AppUser user);
    }
}
