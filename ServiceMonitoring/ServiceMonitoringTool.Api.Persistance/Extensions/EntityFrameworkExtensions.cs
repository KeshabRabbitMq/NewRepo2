using System;
using Microservice.Framework.Domain;
using Microservice.Framework.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceMonitoringTool.Api.Persistance
{
    public static class EntityFrameworkExtensions
    {
        public static IDomainContainer ConfigureEntityFramework(
            this IDomainContainer domainContainer,
            IEntityFrameworkConfiguration configuration)
            {
                if(configuration == null) throw new ArgumentNullException(nameof(configuration));
                configuration.Apply(domainContainer.ServiceCollection);
                return domainContainer;
            }

        public static IDomainContainer AddDbContextProvider<TDbContext,TContextProvider>(
            this IDomainContainer domainContainer)
            where TContextProvider : class, IDbContextProvider<TDbContext>
            where TDbContext : DbContext
            {
                domainContainer
                .ServiceCollection
                .AddTransient<IDbContextProvider<TDbContext>, TContextProvider>();
                return domainContainer;
            }
    }
}