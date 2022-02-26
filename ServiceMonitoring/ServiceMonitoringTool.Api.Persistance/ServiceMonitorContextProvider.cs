using System;
using Microservice.Framework.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ServiceMonitoringTool.Api.Persistance
{
    public class ServiceMonitorContextProvider : IDbContextProvider<ServiceMonitorContext>, IDisposable
    {
        private readonly DbContextOptions<ServiceMonitorContext> _options;

        public ServiceMonitorContextProvider(IConfiguration configuration)
        {
            _options = new DbContextOptionsBuilder<ServiceMonitorContext>()
            .UseSqlServer(configuration["DataConnection:Database"])
            .Options;
        }
        public ServiceMonitorContext CreateContext()
        {
            return new ServiceMonitorContext(_options);
        }

        public void Dispose(){
            
        }
    }
}