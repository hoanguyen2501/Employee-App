namespace EmployeeApp.Service.RabbitMQ
{
    public interface IMessageSender
    {
        void SendMessage<T>(T message);
    }
}
