using System.Collections.Generic;
using Microservice.Framework.Domain.Aggregates;
using Microservice.Framework.Domain.Extensions;

namespace ServiceMonitoringTool.Api.Domain
{
    public class ServiceMonitorAggregate : AggregateRoot<ServiceMonitorAggregate, ServiceMonitorAggregateId>
    {
        #region Constructors

        public ServiceMonitorAggregate()
            : this(null)
        {

        }

        public ServiceMonitorAggregate(ServiceMonitorAggregateId aggregateId)
            : base(aggregateId)
        {

        }

        #endregion

        #region Properties

        public string ServiceName { get; set; }

        public IList<ServiceMethod> ServiceMethods { get; set; }

        public void CreateServiceMonitor(string serviceName)
        {
            Specs.AggregateIsNew.ThrowDomainErrorIfNotSatisfied(this);
            ServiceName = serviceName;
            Emit(new CreatedServiceMonitorEvent(serviceName));
        }

        public void AddServiceMethodEntry(ServiceMethod serviceMethod)
        {
            Specs.AggregateIsCreated.ThrowDomainErrorIfNotSatisfied(this);

            if(ServiceMethods == null)
            {
                ServiceMethods = new List<ServiceMethod>();
            }

            ServiceMethods.Add(serviceMethod);
            Emit(new AddedServiceMethodEntryEvent(serviceMethod));
        }

        #endregion
    }
}