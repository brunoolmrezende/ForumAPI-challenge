using System.Text;
using Forum.Application.UseCases.User.Delete.Delete_User_Account;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Forum.API.BackgroundServices
{
    public class DeleteUserService(string connection, string queueName, IServiceProvider services, ILogger<DeleteUserService> logger) : BackgroundService
    {
        private readonly string _connection = connection;
        private readonly string _queueName = queueName;
        private readonly IServiceProvider _services = services;
        private readonly ILogger<DeleteUserService> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Service is starting.");

            (var channel, var consumer) = await GetConsumerAndChannel(stoppingToken);

            consumer.ReceivedAsync += Consumer_ReceivedAsync;

            await channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

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

        private async Task<(IChannel, AsyncEventingBasicConsumer)> GetConsumerAndChannel(CancellationToken stoppingToken)
        {
            var connection = await CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            return (channel, consumer);
        }

        private async Task<IConnection> CreateConnectionAsync()
        {
            return await new ConnectionFactory { Uri = new Uri(_connection) }.CreateConnectionAsync();
        }
    }
}
