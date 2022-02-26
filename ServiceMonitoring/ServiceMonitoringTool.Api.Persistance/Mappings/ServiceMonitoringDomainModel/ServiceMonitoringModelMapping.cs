using Microsoft.EntityFrameworkCore;
using ServiceMonitoring.Api.Domain;
using ServiceMonitoringTool.Api.Domain;

namespace ServiceMonitoringTool.Api.Persistance
{
    public static class ServiceMonitorModelMapping
    {
        public static ModelBuilder ServiceMonitoringModelMap(this ModelBuilder modelBuilder){
            modelBuilder
            .Entity<ServiceMethod>()
            .Property(o=>o.Id)
            .HasConversion(new SingleValueObjectIdentityValueConverter<ServiceMethodId>());

            //modelBuilder
            //.Entity<ServiceMethod>()
            //.Property(o=>o.ExecutionsStatus)
            //.HasConversion(new ValueObjectValueConverter<ExecutionsStatusType,ExecutionsStatusTypes>());


            modelBuilder
            .Entity<ServiceMonitorAggregate>()
            .Property(o=>o.Id)
            .HasConversion(new SingleValueObjectIdentityValueConverter<ServiceMonitorAggregateId>());


            modelBuilder
            .Entity<ServiceMethod>()
            .HasOne<ServiceMonitorAggregate>()
            .WithMany(c => c.ServiceMethods);

            return modelBuilder;

        }
    }
}