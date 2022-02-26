using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMqCoreLibrary.Interfaces;
using RabbitMqCoreLibrary.Models;
using System;
using System.Text;

namespace RabbitMqCoreLibrary.Services
{
    public class RabbitManager : IRabbitManager
    {
        private readonly DefaultObjectPool<IModel> _objectPool;
        private readonly RabbitOptions _options;

        public RabbitManager(IPooledObjectPolicy<IModel> objectPolicy, IOptions<RabbitOptions> optionsAccs)
        {
            _options = optionsAccs.Value;
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }

        public void Publish<T>(T message)
            where T : class
        {
            if (message == null)
                return;

            var channel = _objectPool.Get();

            try
            {
                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.BasicPublish(_options.ExchangeName, _options.RouteKey, properties, sendBytes);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }
    }
}
