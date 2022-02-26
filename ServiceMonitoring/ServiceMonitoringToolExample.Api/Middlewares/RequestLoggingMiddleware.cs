using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMqCoreLibrary.Interfaces;
using ServiceMonitoringToolExample.Api.Enum;
using ServiceMonitoringToolExample.Api.ExternalServices;
using ServiceMonitoringToolExample.Models;

namespace ServiceMonitoringToolExample.Api.Middlewares{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRabbitManager _manager;
        private readonly ServiceMonitorModel _options;

        public RequestResponseLoggingMiddleware(RequestDelegate next, 
            IRabbitManager manager, 
            IOptions<ServiceMonitorModel> optionsAccs)
        {
            _next = next;
            _manager = manager;
            _options = optionsAccs.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = await FormatRequest(context.Request);
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next(context);
                    var elapsed = stopwatch.Elapsed;
                    var response = await FormatResponse(context.Response);
                    _manager.Publish(
                        new ServiceMethodEntryExternalModel
                        {
                            ServiceId = _options.ServiceId,
                            MethodName = request.Key,
                            RequestUri = request.Value,
                            Response = response.Key,
                            MethodExecutionTime = DateTime.Now,
                            ElapsedTime = stopwatch.Elapsed,
                            ExecutionsStatus = response.Value.ToString(),
                            ExecutedBy = "keshab"
                        });
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            catch (Exception e)
            {
                _manager.Publish(
                        new ServiceMethodEntryExternalModel
                        {
                            ServiceId = _options.ServiceId,
                            MethodName = request.Key,
                            RequestUri = request.Value,
                            Response = e.ToString(),
                            MethodExecutionTime = DateTime.Now,
                            ElapsedTime = stopwatch.Elapsed,
                            ExecutionsStatus = ExecutionsStatus.Ex_Fail.ToString()
                        });

                throw;
            }
        }

        private async Task<KeyValuePair<string, string>> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            return new KeyValuePair<string, string>(request.Path, $"{request.Scheme}://{request.Host}{request.Path} {request.QueryString} {body}");
        }

        private async Task<KeyValuePair<string, ExecutionsStatus>> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return new KeyValuePair<string, ExecutionsStatus>(
                $"{response.StatusCode}: {text}",
                (response.StatusCode >= 400) && (response.StatusCode <= 599)
                ? ExecutionsStatus.Ex_Fail :
                  ExecutionsStatus.Ex_Suc);
        }
    }
}