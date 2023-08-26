using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        public BaseApiController() { }
    }
}