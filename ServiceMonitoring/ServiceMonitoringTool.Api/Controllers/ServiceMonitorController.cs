using Microservice.Framework.Domain.Commands;
using Microservice.Framework.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqCoreLibrary.Models;
using ServiceMonitoringTool.Api.Domain;
using ServiceMonitoringTool.Api.Interfaces;
using ServiceMonitoringTool.Api.Services;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceMonitoringTool.Api
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceMonitorController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;
        private readonly DefaultObjectPool<IModel> _objectPool;
        private readonly RabbitOptions _options;
        private EventingBasicConsumer _consumer;
        private readonly ILogger<ServiceMonitorController> _logger;
        protected IModel _channel { get; private set; }
        private readonly IDbSaveService _dbSaveService;

        public ServiceMonitorController(
            ILogger<ServiceMonitorController> logger,
            ICommandBus commandBus,
            IPooledObjectPolicy<IModel> objectPolicy,
            IOptions<RabbitOptions> optionsAccs,
            IQueryProcessor queryProcessor,
            IDbSaveService dbSaveService
            )
        {
            _logger = logger;
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
            _options = optionsAccs.Value;
            _dbSaveService= dbSaveService;
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }

        [HttpPost("readservicemethodlog")]
        public void ReadServiceMethodLog()
        {
            _channel = _objectPool.Get();
            try
            {
                _consumer = new EventingBasicConsumer(_channel);
                _consumer.Received += _consumer_Received;
                _consumer.Shutdown += _consumer_Shutdown;
                _consumer.Registered += _consumer_Registered;
                _consumer.Unregistered += _consumer_Unregistered;
                _consumer.ConsumerCancelled += _consumer_ConsumerCancelled;
                _channel.BasicConsume(_options.QueueName, false, _consumer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void _consumer_ConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"{nameof(_consumer_ConsumerCancelled)}");
        }

        private void _consumer_Unregistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"{nameof(_consumer_Unregistered)}");
        }

        private void _consumer_Registered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"{nameof(_consumer_Registered)}");
        }

        private void _consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"{nameof(_consumer_Shutdown)}");
        }

        private void _consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var content = Encoding.UTF8.GetString(e.Body.ToArray());
            var data = JsonConvert.DeserializeObject<AddServiceMethodLogRequestModel>(content);
            var addservicemethodlogresult= _dbSaveService.Save(data);

            if (addservicemethodlogresult.IsSuccess)
            {
                _channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
            }
        }



        [HttpPost("addservice")]
        public async Task<IActionResult> AddService(CreateServiceMonitorRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var createServiceMonitorResult = await _commandBus
                    .PublishAsync(
                        new CreateServiceMonitorCommand(ServiceMonitorAggregateId.New, model.ServiceName),
                        CancellationToken.None);

                if (createServiceMonitorResult.IsSuccess)
                    return Ok(createServiceMonitorResult);
                else
                    return BadRequest(createServiceMonitorResult);
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
        }

        [HttpPost("queryservice")]
        public async Task<IActionResult> QueryService(GetServiceMonitorRequestModel model)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _queryProcessor
                    .ProcessAsync(new QueryServiceMonitor(
                        new ServiceMonitorAggregateId(model.Id)),
                        CancellationToken.None));
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
        }
    }
}