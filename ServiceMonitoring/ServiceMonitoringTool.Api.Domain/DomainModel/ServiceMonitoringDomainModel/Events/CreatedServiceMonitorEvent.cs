using Microservice.Framework.Domain.Events;
using Microservice.Framework.Domain.Events.AggregateEvents;

namespace ServiceMonitoringTool.Api.Domain
{
    [EventVersion("CreatedServiceMonitor", 1)]
    public class CreatedServiceMonitorEvent : AggregateEvent<ServiceMonitorAggregate, ServiceMonitorAggregateId>
    {
        public CreatedServiceMonitorEvent(string serviceName)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; }
    }
}