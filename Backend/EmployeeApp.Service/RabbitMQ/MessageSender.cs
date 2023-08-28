using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace EmployeeApp.Service.RabbitMQ
{
    public class MessageSender : IMessageSender
    {
        private readonly IHostApplicationLifetime lifetime;

        public MessageSender(IHostApplicationLifetime lifetime)
        {
            this.lifetime = lifetime;
        }

        public void SendMessage<T>(T message)
        {
            ConnectionFactory factory = new()
            {
                Uri = new Uri("amqp:guest:guest@localhost:5672"),
                ClientProvidedName = "Rabbit Sender EmployeeApp"
            };

            IConnection connection = factory.CreateConnection();

            IModel channel = connection.CreateModel();

            string exchangeName = "EmployeeAppExchange";
            string routingKey = "employee-app-routing-key";
            string queueName = "EmployeeAppQueue";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, false, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);

            string messageString = JsonConvert.SerializeObject(message);
            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(messageString);
            channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

            lifetime.ApplicationStopping.Register(() =>
            {
                channel.Close();
                connection.Close();
            });
        }
    }
}
