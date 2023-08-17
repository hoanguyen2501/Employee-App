using EmployeeApp.Service.DTOs.AppUser;
using EmployeeApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    [Route("api/auth")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUserDto>> Login(AppUserLoginDto loginDto)
        {
            var userDto = await _accountService.Login(loginDto);
            if (userDto == null)
                return BadRequest("Username or Password is invalid");

            return Ok(userDto);
        }
    }
}
