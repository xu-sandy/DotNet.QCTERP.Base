using Qct.Infrastructure.MessageClient.Implementations;
using Qct.Infrastructure.MessageClient.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.MessageClient
{
    interface IRemoteServiceAgent : IEventHandler<EventJsonWrapper>
    {
        void Subscribe(SubscribeItem item);

        EventHandler RetryAllSubscribes { get; set; }
    }
}
