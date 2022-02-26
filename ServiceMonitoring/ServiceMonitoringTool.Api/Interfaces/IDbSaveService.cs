using Microservice.Framework.Domain.ExecutionResults;

namespace ServiceMonitoringTool.Api.Interfaces
{
    public interface IDbSaveService
    {
        public IExecutionResult Save(AddServiceMethodLogRequestModel data);
    }
}
