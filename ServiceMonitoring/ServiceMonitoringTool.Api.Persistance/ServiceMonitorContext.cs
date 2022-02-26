
using Microsoft.EntityFrameworkCore;

namespace ServiceMonitoringTool.Api.Persistance
{
    public class ServiceMonitorContext :  DbContext
    {
        public ServiceMonitorContext(DbContextOptions<ServiceMonitorContext> options) : 
        base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ServiceMonitoringModelMap();
        }

    }
}