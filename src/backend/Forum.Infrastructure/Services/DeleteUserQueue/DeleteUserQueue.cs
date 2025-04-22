using Forum.Domain.Entities;
using Forum.Domain.Services;
using RabbitMQ.Client;
using System.Text;

namespace Forum.Infrastructure.Services.DeleteUserQueue
{
    public class DeleteUserQueue : IDeleteUserQueue
    {
        private readonly string _connection;
        private readonly string _queueName;
        private readonly ConnectionFactory _factory;

        public DeleteUserQueue(string connection, string queueName)
        {
            _connection = connection;
            _queueName = queueName;
            _factory = new ConnectionFactory { Uri = new Uri(_connection) };
        }

        public async Task SendMessage(User user)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = user.UserIdentifier.ToString();
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: _queueName,
                body: body);
        }
    }
}
