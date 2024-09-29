using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace AdventureWorksQueryPerformance.Service
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "query_results",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                 routingKey: "query_results",
                                 basicProperties: null,
                                 body: body);
            Log.Information(" [x] Sent " + message);
        }

        public void Close()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
