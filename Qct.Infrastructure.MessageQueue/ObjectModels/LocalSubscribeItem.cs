using System;
using System.Linq;

namespace Qct.Infrastructure.MessageClient.ObjectModels
{
    /// <summary>
    /// 订阅项
    /// </summary>
    public class LocalSubscribeItem
    {
        /// <summary>
        /// 事件处理器（事件回调处理）
        /// </summary>
        internal IEventHandler Handler { get; set; }
        /// <summary>
        /// 订阅过滤模式
        /// </summary>
        public FilterMode FilterMode { get; set; }
        /// <summary>
        /// 订阅内容描述
        /// </summary>
        public string Descriptions { get; set; }
        /// <summary>
        /// 订阅编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 是否取消订阅
        /// </summary>
        public bool IsUnSubscribe { get; set; }

        public bool IsActive(IEvent domainEvent, bool isLocalLoop, Guid currentPublisherId)
        {
            bool isActive = false;
            switch (FilterMode)
            {
                case FilterMode.WholeMatched:
                    {
                        if (domainEvent.Descriptions == Descriptions)
                        {
                            isActive = true;
                        }
                    }
                    break;
                case FilterMode.StartsWith:
                    {
                        if (domainEvent.Descriptions.StartsWith(Descriptions))
                        {
                            isActive = true;
                        }
                    }
                    break;
                    //case FilterMode.FullLocalEvent:
                    //    {
                    //        if (Descriptions == "*" && domainEvent.PublisherId == currentPublisherId)//只能匹配内部事件
                    //        {
                    //            isActive = true;
                    //        }
                    //    }
                   // break;
            }
            return isActive;
        }
    }
}
