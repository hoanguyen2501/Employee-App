using AutoMapper;
using EmployeeApp.Service.DTOs.AppUser;
using EmployeeApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly RabbitMqController _rabbitMqController;

        public AuthController(IAuthService accountService, RabbitMqController rabbitMqController, IMapper mapper)
        {
            _authService = accountService;
            _rabbitMqController = rabbitMqController;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(AuthRequest loginDto)
        {
            AppUserDto appUser = await _authService.Login(loginDto);
            if (appUser == null)
                return BadRequest("Username or Password is invalid");

            AuthResponse authResponse = _mapper.Map<AuthResponse>(appUser);

            Response.Cookies.Append(
                "X-Username",
                authResponse.Username,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append(
                "X-Access-Token",
                authResponse.AccessToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append(
                "X-Refresh-Token",
                appUser.RefreshToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });

            var message = new
            {
                host = Request.Host.ToString(),
                path = Request.Path,
                method = Request.Method,
                title = Request.RouteValues,
                statusCode = Response.StatusCode,
                message = "Logged in successfully",
                data = appUser,
            };
            _rabbitMqController.CreateMessage(message);

            return Ok(authResponse);
        }

        [Authorize]
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh()
        {
            if (!(Request.Cookies.TryGetValue("X-Username", out string username) &&
                Request.Cookies.TryGetValue("X-Refresh-Token", out string refreshToken)))
                return Unauthorized("Your request is declined");

            AppUserDto refreshedAppUser = await _authService.Refresh(username, refreshToken);

            AuthResponse authResponse = _mapper.Map<AuthResponse>(refreshedAppUser);

            if (authResponse == null)
                return Unauthorized("Refresh token is invalid");

            Response.Cookies.Append(
                "X-Username",
                authResponse.Username,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append(
                "X-Access-Token",
                authResponse.AccessToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append(
                "X-Refresh-Token",
                refreshedAppUser.RefreshToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });

            var message = new
            {
                host = Request.Host.ToString(),
                path = Request.Path,
                method = Request.Method,
                title = Request.RouteValues,
                statusCode = Response.StatusCode,
                message = "Refresh access token successfully",
                data = refreshedAppUser,
            };
            _rabbitMqController.CreateMessage(message);

            return Ok(authResponse);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            if (!(Request.Cookies["X-Username"] != null &&
                Request.Cookies.TryGetValue("X-Refresh-Token", out string refreshToken)))
                return Unauthorized("Your request is declined");

            bool isLoggedout = await _authService.Logout(refreshToken);

            if (isLoggedout)
            {
                Response.Cookies.Delete("X-Username");
                Response.Cookies.Delete("X-Access-Token");
                Response.Cookies.Delete("X-Refresh-Token");
                return NoContent();
            }

            return BadRequest();
        }
    }
}
