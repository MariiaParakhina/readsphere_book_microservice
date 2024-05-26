using System.Text;
using Core;
using Core.UseCases;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BookService
{
    public class DeleteUserBackgroundService : BackgroundService
    {
        private readonly IConnectionFactory _factory;
        private readonly IServiceProvider _serviceProvider;
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

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare("delete_user_queue",
                true,
                false,
                false,
                null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    await ProcessMessage(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            _channel.BasicConsume("delete_user_queue",
                true,
                consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken); // Keeps the service running
            }
        }

        private async Task ProcessMessage(string message)
        {
            byte[] encryptedUserId = JsonConvert.DeserializeObject<byte[]>(message) ?? throw new ArgumentNullException(nameof(message));

            using (var scope = _serviceProvider.CreateScope())
            {
                int userId = int.Parse(StringDecryptor.Decrypt(encryptedUserId));
                var useCase = scope.ServiceProvider.GetRequiredService<DeleteUserDataUseCase>();

                // Implement the delete user use case
                await useCase.Execute(userId);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _channel?.Close();
            _connection?.Close();
            await base.StopAsync(cancellationToken);
        }
    }
}