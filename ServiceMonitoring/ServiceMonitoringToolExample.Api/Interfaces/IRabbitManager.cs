using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqCoreLibrary.Interfaces
{
    public interface IRabbitManager
    {
        void Publish<T>(T message)
            where T : class;
    }
}
