using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Collections.Concurrent;

namespace WebApp
{
    public class RabbitMqConsumer
    {
        private readonly IModel _channel;
        private readonly ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();

        public RabbitMqConsumer()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password"
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: "query_results",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void StartConsuming(Action<string> onMessageReceived)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _messages.Enqueue(message);

                if (_messages.Count > 5)
                {
                    _messages.TryDequeue(out _);
                }

                onMessageReceived(message);
            };

            _channel.BasicConsume(queue: "query_results", autoAck: true, consumer: consumer);
        }

        public IEnumerable<string> GetLastMessages()
        {
            return _messages.ToArray();
        }
    }
}
