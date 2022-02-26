using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMqCoreLibrary.Models;
using System;

namespace RabbitMqCoreLibrary.Services
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>, IDisposable
    {
        private readonly RabbitOptions _options;

        private IConnection _connection;

        private readonly ILogger<RabbitModelPooledObjectPolicy> _logger;
        protected IModel _channel { get; private set; }

        public RabbitModelPooledObjectPolicy(IOptions<RabbitOptions> optionsAccs, ILogger<RabbitModelPooledObjectPolicy> logger)
        {
            _options = optionsAccs.Value;
            _logger = logger;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _options.HostName,
                    UserName = _options.UserName,
                    Password = _options.Password,
                    Port = _options.Port,
                    VirtualHost = _options.VHost,
                    DispatchConsumersAsync = _options.DispatchConsumersAsync,
                    AutomaticRecoveryEnabled = _options.AutomaticRecoveryEnabled,
                };

                if (_connection == null || _connection.IsOpen == false)
                {
                    _connection =  factory.CreateConnection();
                    _connection.ConnectionShutdown += _connection_ConnectionShutdown;
                }
                return _connection;
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to connect to RabbitMq server", e);
                throw;
            }
        }

        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Dispose();
        }

        public IModel Create()
        {
            if (_channel == null || _channel.IsOpen == false)
            {
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(
                        exchange: _options.ExchangeName,
                        type: _options.ExchangeType,
                        durable: true,
                        autoDelete: false);

                _channel.QueueDeclare(
                    queue: _options.QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false);

                _channel.QueueBind(
                    queue: _options.QueueName,
                    exchange: _options.ExchangeName,
                    routingKey: _options.RouteKey);

                _channel.BasicQos(
                    prefetchSize: 0,
                    prefetchCount: 1,
                    global: false
                    );
            }
            return _channel;
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
