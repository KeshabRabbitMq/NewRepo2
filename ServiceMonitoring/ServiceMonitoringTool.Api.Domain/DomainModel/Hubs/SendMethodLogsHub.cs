using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ServiceMonitoringTool.Api.Domain
{
    public class SendMethodLogsHub : Hub<ISendMethodLogsHub>
    {
        public Task SendMethodLog(object data, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}