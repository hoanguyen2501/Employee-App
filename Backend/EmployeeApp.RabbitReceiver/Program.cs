using NLog;
using NLog.Web;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Init receiver server");

try
{
    ConnectionFactory factory = new()
    {
        Uri = new Uri("amqp:guest:guest@localhost:5672"),
        ClientProvidedName = "Rabbit Receiver EmployeeApp"
    };

    IConnection connection = factory.CreateConnection();

    IModel channel = connection.CreateModel();

    string exchangeName = "EmployeeAppExchange";
    string routingKey = "employee-app-routing-key";
    string queueName = "EmployeeAppQueue";

    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
    channel.QueueDeclare(queueName, false, false, false, null);
    channel.QueueBind(queueName, exchangeName, routingKey, null);
    channel.BasicQos(0, 1, false);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, args) =>
    {
        Task.Delay(TimeSpan.FromSeconds(5)).Wait();

        var body = args.Body.ToArray();

        string message = Encoding.UTF8.GetString(body);
        logger.Info(message);

        channel.BasicAck(args.DeliveryTag, false);
    };

    string consumerTag = channel.BasicConsume(queueName, false, consumer);
    Console.ReadLine();

    channel.BasicCancel(consumerTag);

    channel.Close();
    connection.Close();
}
catch (Exception exception)
{
    logger.Error(exception, "Some errors occurred during starting program");
    throw;
}
finally
{
    LogManager.Shutdown();
}