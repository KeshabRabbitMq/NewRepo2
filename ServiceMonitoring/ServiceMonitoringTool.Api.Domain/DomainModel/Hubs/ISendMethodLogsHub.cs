using System.Threading;
using System.Threading.Tasks;

namespace ServiceMonitoringTool.Api.Domain
{
    public interface ISendMethodLogsHub
    {
        Task SendMethodLog(CancellationToken cancellationToken);
    }
}