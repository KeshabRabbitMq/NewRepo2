using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMqCoreLibrary.Models;
using System;
using System.Net.Security;
using System.Security.Authentication;

namespace RabbitMqCoreLibrary.Services
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitOptions _options;

        private IConnection _connection;

        private readonly ILogger<RabbitModelPooledObjectPolicy> _logger;
        protected IModel Channel { get; private set; }



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
                }
                return _connection;
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to connect to RabbitMq server", e);
                throw;
            }
        }

        public IModel Create()
        {
            if (Channel == null || Channel.IsOpen == false)
            {
                Channel = _connection.CreateModel();
                Channel.ExchangeDeclare(
                        exchange: _options.ExchangeName,
                        type: _options.ExchangeType,
                        durable: true,
                        autoDelete: false);

                Channel.QueueDeclare(
                    queue: _options.QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false);

                Channel.QueueBind(
                    queue: _options.QueueName,
                    exchange: _options.ExchangeName,
                    routingKey: _options.RouteKey);
            }
            return Channel;
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
    }
}
