using System.Reflection;
using Microservice.Framework.Domain;
using Microservice.Framework.Domain.Extensions;
using Microsoft.Extensions.Configuration;

namespace ServiceMonitoringTool.Api.Domain
{
    public static class ServiceMonitoringDomainExtension
    {
        public static Assembly Assembly { get; } = typeof(ServiceMonitoringDomainExtension).Assembly;

        public static IDomainContainer ConfigureServiceMonitoringDomain(
            this IDomainContainer domainContainer,
            IConfiguration configuration = null)
        {
            return domainContainer
                .AddDefaults(Assembly);
        }
    }
}