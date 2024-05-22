using System.Text;
using Core;
using Core.UseCases;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class DeleteUserBackgroundService : BackgroundService
{
    private readonly IConnectionFactory _factory;
    private readonly IServiceProvider _serviceProvider;
    private readonly object _lock = new();
    private IConnection? _connection;
    private IModel? _channel;

    public DeleteUserBackgroundService(IServiceProvider serviceProvider)
    {
        _factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER"),
            Port = 5672,
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")
        };
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _connection = _connection ?? _factory.CreateConnection();
        _channel = _channel ?? _connection.CreateModel();

        lock (_lock)
        {
            if (!_channel.IsOpen) _channel = _connection.CreateModel();
        }

        _channel.QueueDeclare("delete_user_queue",
            true,
            false,
            false,
            null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            try
            {
                ProcessMessage(ea);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        };

        _channel.BasicConsume("delete_user_queue",
            true,
            consumer);

        await Task.CompletedTask;
    }

    private async Task ProcessMessage(BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        byte[] encryptedUserId = JsonConvert.DeserializeObject<byte[]>(message) ?? throw new ArgumentNullException("JsonDeserializer.Deserialize<byte[]>(message)");
        
        using (var scope = _serviceProvider.CreateScope())
        {
            int userId = int.Parse(StringDecryptor.Decrypt(encryptedUserId));
            DeleteUserDataUseCase? useCase = scope.ServiceProvider.GetService<DeleteUserDataUseCase>();
            
            // implement delete user use case 
            _ = useCase.Execute(userId);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        _connection?.Close();
        await base.StopAsync(cancellationToken);
    }
}
