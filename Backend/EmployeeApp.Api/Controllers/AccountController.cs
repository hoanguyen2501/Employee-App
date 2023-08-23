using EmployeeApp.Service.DTOs.AppUser;
using EmployeeApp.Service.Interfaces;
using EmployeeApp.Service.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    [Route("api/auth")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService, IMessageSender sender) : base(sender)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUserDto>> Login(AppUserLoginDto loginDto)
        {
            AppUserDto userDto = await _accountService.Login(loginDto);
            if (userDto == null)
                return BadRequest("Username or Password is invalid");

            CreateMessage("Logged in to system successfully", userDto);
            //var message = new
            //{
            //    domain = HttpContext.Request.Host.ToString(),
            //    path = HttpContext.Request.Path,
            //    method = HttpContext.Request.Method,
            //    title = HttpContext.Request.RouteValues,
            //    statusCode = HttpContext.Response.StatusCode,
            //    message = "Logged in to system successfully",
            //    value = userDto,
            //};

            //_sender.SendMessage(message);
            return Ok(userDto);
        }
    }
}
