using System.Text;
using RabbitMQ.Client;

namespace BookService.Services;

public interface IMessageQueueService
{
    void PublishMessage(string queueName, string message);
}

public class RabbitMqService : IMessageQueueService
{
    private readonly ConnectionFactory _factory;

    public RabbitMqService()
    {
        _factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER"),
            Port = 5672,
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")
        };
    }

    public void PublishMessage(string queueName, string message)
    {
        using (var connection = _factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queueName,
                true,
                false,
                false,
                null);

            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish("",
                queueName,
                properties,
                body);
        }
    }
}