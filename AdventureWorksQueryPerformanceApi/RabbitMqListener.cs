using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace AdventureWorksQueryPerformanceApi
{
    public class RabbitMqListener
    {
        private readonly IHubContext<QueryResultHub> _hubContext;

        public RabbitMqListener(IHubContext<QueryResultHub> hubContext)
        {
            _hubContext = hubContext;
            StartListening();
        }

        public void StartListening()
        {
            var factory = new ConnectionFactory() { 
                HostName = "localhost", 
                UserName = "user", 
                Password = "password"
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "query_results",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await _hubContext.Clients.All.SendAsync("ReceiveQueryResult", message);
            };

            channel.BasicConsume(queue: "query_results",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
