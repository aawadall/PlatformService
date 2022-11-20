using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly ILogger<MessageBusClient> _logger;
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration, ILogger<MessageBusClient> logger)
        {
            _logger = logger;
            _configuration = configuration;
            int port;
            int.TryParse(_configuration["RabbitMQPort"],out port);
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQHost"],
                Port = port
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                _logger.LogInformation($"--> Connected to Message Bus");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"---> Could not connect to RabbitMQ: {ex.Message}");
            }

        }
        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            _logger.LogDebug($"--> Publishing new platform to Message Bus");
            var message = JsonSerializer.Serialize(platformPublishedDto);
            
            if (_connection.IsOpen)
            {
                _logger.LogDebug($"--> RabbitMQ connection is open, sending message");
                SendMessage(message);
            }
            else
            {
                _logger.LogError("---> RabbitMQ connection is closed");
            }
        }

        public void Dispose()
        {
            _logger.LogInformation($"--> MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body
            );
            _logger.LogInformation($"--> We have sent {message}");
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("---> RabbitMQ Connection Shutdown");
        }
    }
}
