using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMqCoreLibrary.Interfaces;
using RabbitMqCoreLibrary.Models;
using RabbitMqCoreLibrary.Services;
using ServiceMonitoringToolExample.Models;

namespace RabbitMqCoreLibrary.Extension
{
    public static class RabbitServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbit(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitConfig = configuration.GetSection("rabbit");
            var serviceConfig = configuration.GetSection("ServiceMonitor");
            services.Configure<RabbitOptions>(rabbitConfig);
            services.Configure<ServiceMonitorModel>(serviceConfig);

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();

            services.AddSingleton<IRabbitManager, RabbitManager>();

            return services;
        }
    }
}
