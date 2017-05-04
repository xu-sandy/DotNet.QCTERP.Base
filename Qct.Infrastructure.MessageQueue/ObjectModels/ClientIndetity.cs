#if MQM

using MongoDB.Bson;
#endif

using System;

namespace Qct.Infrastructure.MessageClient.ObjectModels
{
    public class ClientIndetity
    {
#if MQM
        public ObjectId _id { get; set; }
        public Guid AgentId { get; set; }

#endif
        /// <summary>
        /// 事件发布器Id
        /// </summary>
        public Guid PublisherId { get; set; }

        public string SessionId { get; set; }

        public bool IsWebSite { get; set; }

        public string WebSiteHost { get; set; }
    }
}
