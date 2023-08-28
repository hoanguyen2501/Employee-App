using EmployeeApp.Service.RabbitMQ;

namespace EmployeeApp.Api.Controllers
{
    public class RabbitMqController : BaseApiController
    {
        private readonly IMessageSender _sender;

        public RabbitMqController(IMessageSender sender)
        {
            _sender = sender;
        }

        public void CreateMessage(object message)
        {
            _sender.SendMessage(message);
        }
    }
}
