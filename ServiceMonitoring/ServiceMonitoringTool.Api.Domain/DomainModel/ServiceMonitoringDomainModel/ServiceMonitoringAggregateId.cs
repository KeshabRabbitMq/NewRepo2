using System.Text.Json.Serialization;
using Microservice.Framework.Common;

namespace ServiceMonitoringTool.Api.Domain
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ServiceMonitorAggregateId : Identity<ServiceMonitorAggregateId>
    {
        public ServiceMonitorAggregateId(string value) : base(value)
        {
        }
    }
}