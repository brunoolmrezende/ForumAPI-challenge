using Forum.Application.UseCases.User.Delete.Delete_User_Account;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Forum.API.BackgroundServices
{
    public class DeleteUserService : BackgroundService
    {
        private readonly string _connection;
        private readonly string _queueName;
        private readonly ConnectionFactory _factory;
        private readonly IServiceProvider _services;
        private readonly ILogger<DeleteUserService> _logger;

        public DeleteUserService(string connection, string queueName, IServiceProvider services, ILogger<DeleteUserService> logger)
        {
            _logger = logger;
            _connection = connection;
            _queueName = queueName;
            _factory = new ConnectionFactory { Uri = new Uri(_connection) };
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DeleteUserService started...");

            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += Consumer_ReceivedAsync;

            await channel.BasicConsumeAsync(
                 queue: _queueName,
                 autoAck: false,
                 consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation($"Message received: {message}");

            var channel = ((AsyncEventingBasicConsumer)sender).Channel;

            try
            {
                var userIdentifier = Guid.Parse(message);
                _logger.LogInformation("Processing identifier.");

                using var scope = _services.CreateScope();
                var deleteUserUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();

                await deleteUserUseCase.Execute(userIdentifier);

                await channel.BasicAckAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                _logger.LogInformation("Ack sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
                await channel.BasicNackAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: false);
            }
        }
    }
}
