using System;

namespace ServiceMonitoringTool.Api
{
    public class AddServiceMethodLogRequestModel
    {
        public string ServiceId { get; set; }
        public string MethodName { get; set; }
        public string RequestUri { get; set; }
        public string Response { get; set; }
        public DateTime MethodExecutionTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public string ExecutionsStatus { get; set; }
        public string ExecutedBy { get; set; }
    }
}