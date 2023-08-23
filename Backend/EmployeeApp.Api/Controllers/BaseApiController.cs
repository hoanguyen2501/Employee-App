using EmployeeApp.Service.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private readonly IMessageSender _sender;

        public BaseApiController(IMessageSender sender)
        {
            _sender = sender;
        }

        protected void CreateMessage(string messageString, object value)
        {
            var message = new
            {
                host = HttpContext.Request.Host.ToString(),
                path = HttpContext.Request.Path,
                method = HttpContext.Request.Method,
                title = HttpContext.Request.RouteValues,
                statusCode = HttpContext.Response.StatusCode,
                message = messageString,
                value,
            };

            _sender.SendMessage(message);
        }
    }
}