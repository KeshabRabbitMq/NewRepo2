using Microservice.Framework.Domain.Commands;
using Microservice.Framework.Domain.ExecutionResults;
using ServiceMonitoringTool.Api.Domain;
using ServiceMonitoringTool.Api.Interfaces;
using System;
using System.Threading;


namespace ServiceMonitoringTool.Api.Services
{
    public class DbSaveService: IDbSaveService
    {
        private readonly ICommandBus _commandBus;
        public DbSaveService(
            ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public IExecutionResult Save(AddServiceMethodLogRequestModel data)
        {
            try
            {
                var addservicemethodlogresult = _commandBus.PublishAsync(new AddServiceMethodEntryCommand(
                            new ServiceMonitorAggregateId(data.ServiceId),
                            new ServiceMethod
                            {
                                Id = ServiceMethodId.New,
                                Name = data.MethodName,
                                Request = data.RequestUri,
                                Response = data.Response,
                                ExecutionTime = data.MethodExecutionTime,
                                TimeElapsed = data.ElapsedTime,
                                ExecutionsStatus = data.ExecutionsStatus,
                                ExecutedBy = data.ExecutedBy
                            }), CancellationToken.None).Result;

                return addservicemethodlogresult;

                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
