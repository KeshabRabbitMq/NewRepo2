using System.Text.Json.Serialization;
using Microservice.Framework.Common;

namespace ServiceMonitoringTool.Api.Domain
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ServiceMethodId : Identity<ServiceMethodId>
    {
        public ServiceMethodId(string value)
            : base(value)
        {

        }
    }
}