using Qct.Infrastructure.Data.EntityInterface;
using Qct.Infrastructure.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.MessageClient.Implementations
{

    /// <remarks>用于网络传输封装</remarks>
    public class EventJsonWrapper : BaseEvent, GuidIdEntity
    {
        public EventJsonWrapper() { }
        public EventJsonWrapper(IEvent eventObject)
        {
            EventObject = JsonHelper.ToJson(eventObject);
            PublisherId = eventObject.PublisherId;
            Descriptions = eventObject.Descriptions;
            CreateDt = eventObject.CreateDt;
            Id = eventObject.Id;
        }

        public string EventObject { get; set; }

        public bool IsLocalLoop { get; set; }

        public override string ToString()
        {
            return EventObject;
        }
    }
}
