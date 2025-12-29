using System.Text;
using Contracts.Service.Core;
using Model.Request.RabitMQ;
using Model.Static;
using RabbitMQ.Client;
using Service.Base;
using WebApp;

namespace Service.Core
{
    public class RabitMQProducerService : BaseService, IRabitMQProducerService
    {
        private Dictionary<string, IConnection> _connections = new Dictionary<string, IConnection>();
        private readonly object _lock = new();
        public RabitMQProducerService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<bool> SendEmailAsync(RabbitMQSendEmailRequest request)
        {
            return this.SendMessages(new List<RabbitMQSendEmailRequest>() { request }, AppSettings.RabbitMqEmail);
        }
        private IConnection GetOrCreateConnection(RabbitMqConfiguration configuration)
        {
            var configKey = Newtonsoft.Json.JsonConvert.SerializeObject(configuration);

            lock (_lock)
            {
                if (!_connections.ContainsKey(configKey))
                {
                    var factory = this.CreateFactory(configuration);
                    var connection = factory.CreateConnection();
                    _connections[configKey] = connection;
                }

                return _connections[configKey];
            }
        }
        private ConnectionFactory CreateFactory(RabbitMqConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                VirtualHost = configuration.VirtualHost,
                HostName = configuration.HostName,
                UserName = configuration.UserName,
                Password = configuration.Password,
                Port = configuration.Port
            };
            return factory;
        }
        public bool SendMessages<T>(IEnumerable<T> messages, RabbitMqConfiguration configuration)
        {
            try
            {
                var connection = GetOrCreateConnection(configuration);
                using var channel = connection.CreateModel();

                channel.QueueDeclare(configuration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                foreach (var message in messages)
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(message);
                    var body = Encoding.UTF8.GetBytes(json);
                    channel.BasicPublish(exchange: string.Empty, routingKey: configuration.QueueName, basicProperties: null, body: body);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogWriter.Writer(ex.Message, "RabitMQProducerService", "");
                return false;

            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                foreach (var connection in _connections.Values)
                {
                    if (connection.IsOpen)
                    {
                        connection.Close();
                    }
                    connection.Dispose();
                }

                _connections.Clear();
            }

        }

        public bool SendMessage<T>(T message, RabbitMqConfiguration configuration)
        {
            try
            {
                var connection = GetOrCreateConnection(configuration);
                using var channel = connection.CreateModel();

                channel.QueueDeclare(configuration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var json = System.Text.Json.JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: string.Empty, routingKey: configuration.QueueName, basicProperties: null, body: body);
                LogWriter.Writer(json, $"Push Message Success to {configuration.QueueName}", "");
                return true;
            }
            catch (Exception ex)
            {
                LogWriter.Writer(ex.Message, "RabitMQProducerService", "");
                return false;

            }
        }
    }
}