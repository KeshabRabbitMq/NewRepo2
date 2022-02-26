using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqCoreLibrary.Models
{
    public class RabbitOptions
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public string VHost { get; set; }

        public bool DispatchConsumersAsync { get; set; }

        public bool AutomaticRecoveryEnabled { get; set; }

        public string ExchangeName { get; set; }

        public string ExchangeType { get; set; }

        public string QueueName { get; set; }
        public string RouteKey { get; set; }
    }
}
