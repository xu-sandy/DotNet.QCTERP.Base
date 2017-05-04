#if MQM
using MongoDB.Bson;
#endif

using System;

namespace Qct.Infrastructure.MessageClient.ObjectModels
{
    public class SubscribeItem
    {
#if MQM
        public ObjectId _id { get; set; }
#endif
        /// <summary>
        /// 订阅描述
        /// </summary>
        public string Descriptions { get; set; }
        /// <summary>
        /// 订阅过滤模式
        /// </summary>
        public FilterMode FilterMode { get; set; }
        /// <summary>
        /// 事件发布器Id
        /// </summary>
        public Guid PublisherId { get; set; }
    }
}
