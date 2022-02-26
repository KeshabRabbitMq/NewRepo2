using System.Threading;
using System.Threading.Tasks;
using Microservice.Framework.Domain.Commands;
using Microservice.Framework.Domain.ExecutionResults;

namespace ServiceMonitoringTool.Api.Domain
{
    public class CreateServiceMonitorCommand 
        : Command<ServiceMonitorAggregate, ServiceMonitorAggregateId>
    {
        public CreateServiceMonitorCommand(
            ServiceMonitorAggregateId aggregateId,
            string serviceName)
            : base(aggregateId)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; }
    }

    public class CreateServiceMonitorCommandHandler 
        : CommandHandler<ServiceMonitorAggregate, ServiceMonitorAggregateId, CreateServiceMonitorCommand>
    {
        public override Task<IExecutionResult> ExecuteAsync(
            ServiceMonitorAggregate aggregate, 
            CreateServiceMonitorCommand command, 
            CancellationToken cancellationToken)
        {
            aggregate.CreateServiceMonitor(command.ServiceName);

            return Task.FromResult(ExecutionResult.Success());
        }
    }
}