using Microservice.Framework.Domain.Events;
using Microservice.Framework.Domain.Events.AggregateEvents;

namespace ServiceMonitoringTool.Api.Domain
{
    [EventVersion("AddedServiceMethodEntry", 1)]
    public class AddedServiceMethodEntryEvent : AggregateEvent<ServiceMonitorAggregate, ServiceMonitorAggregateId>
    {
        public AddedServiceMethodEntryEvent(ServiceMethod serviceMethod)
        {
            ServiceMethod = serviceMethod;
        }

        public ServiceMethod ServiceMethod { get; }
    }
}