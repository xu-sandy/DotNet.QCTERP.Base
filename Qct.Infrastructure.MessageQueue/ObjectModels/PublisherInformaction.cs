using System;

namespace Qct.Infrastructure.MessageClient.ObjectModels
{
    public class PublisherInformaction
    {
        /// <summary>
        /// 事件发布器Id
        /// </summary>
        public Guid PublisherId { get; set; }

        public string SessionId { get; set; }

        public bool IsWebSite { get; set; }

        public string WebSiteHost { get; set; }

        public string MQMToken { get; set; }
    }
}
