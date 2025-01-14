﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMqCoreLibrary.Models;
using RabbitMqCoreLibrary.Services;

namespace RabbitMqCoreLibrary.Extension
{
    public static class RabbitServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbit(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitConfig = configuration.GetSection("rabbit");
            services.Configure<RabbitOptions>(rabbitConfig);

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();

            return services;
        }
    }
}
