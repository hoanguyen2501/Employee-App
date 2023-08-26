using EmployeeApp.Service.DTOs.AppUser;
using EmployeeApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    [Route("api/auth")]
    public class AccountController : BaseApiController
    {
        private readonly IAutheService _accountService;
        private readonly RabbitMqController _rabbitMqController;

        public AccountController(IAutheService accountService, RabbitMqController rabbitMqController)
        {
            _accountService = accountService;
            _rabbitMqController = rabbitMqController;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUserDto>> Login(AppUserLoginDto loginDto)
        {
            AppUserDto newAppUser = await _accountService.Login(loginDto);
            if (newAppUser == null)
                return BadRequest("Username or Password is invalid");

            HttpContext.Response.Cookies.Append(
                "X-Username",
                newAppUser.Username,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            HttpContext.Response.Cookies.Append(
                "X-Access-Token",
                newAppUser.AccessToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            HttpContext.Response.Cookies.Append(
                "X-Refresh-Token",
                newAppUser.RefreshToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });

            var message = new
            {
                host = HttpContext.Request.Host.ToString(),
                path = HttpContext.Request.Path,
                method = HttpContext.Request.Method,
                title = HttpContext.Request.RouteValues,
                statusCode = HttpContext.Response.StatusCode,
                message = "Logged in successfully",
                data = newAppUser,
            };
            _rabbitMqController.CreateMessage(message);

            return Ok(newAppUser);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AppUserDto>> Refresh()
        {
            if (!(Request.Cookies.TryGetValue("X-Username", out string username) &&
                Request.Cookies.TryGetValue("X-Refresh-Token", out string refreshToken)))
                return BadRequest("Your request is declined");

            AppUserRefreshDto refreshUserDto = new() { Username = username, RefreshToken = refreshToken };

            AppUserDto refreshedAppUser = await _accountService.Refresh(refreshUserDto);

            if (refreshedAppUser == null)
                return BadRequest("Refresh token is invalid");

            Response.Cookies.Append(
                "X-Username",
                refreshedAppUser.Username,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append(
                "X-Access-Token",
                refreshedAppUser.AccessToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append(
                "X-Refresh-Token",
                refreshedAppUser.RefreshToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });

            var message = new
            {
                host = HttpContext.Request.Host.ToString(),
                path = HttpContext.Request.Path,
                method = HttpContext.Request.Method,
                title = HttpContext.Request.RouteValues,
                statusCode = HttpContext.Response.StatusCode,
                message = "Refresh access token successfully",
                data = refreshedAppUser,
            };
            _rabbitMqController.CreateMessage(message);

            return Ok(refreshedAppUser);
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("X-Username");
            Response.Cookies.Delete("X-Access-Token");
            Response.Cookies.Delete("X-Refresh-Token");

            return Ok();
        }
    }
}
