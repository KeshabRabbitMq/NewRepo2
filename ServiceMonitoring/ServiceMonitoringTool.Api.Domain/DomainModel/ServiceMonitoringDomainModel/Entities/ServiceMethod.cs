using System;
using Microservice.Framework.Domain;

namespace ServiceMonitoringTool.Api.Domain
{
    public class ServiceMethod : Entity<ServiceMethodId>
    {
        #region Properties

        public string Name { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public DateTime ExecutionTime { get; set; }

        public TimeSpan TimeElapsed { get; set; }

        public string ExecutionsStatus { get; set; }

        public string ExecutedBy { get; set; }

        #endregion
    }
}