using System;

namespace Qct.Infrastructure.MessageClient.ObjectModels
{
    public class PublishItem
    {
        public string Content { get; set; }

        /// <summary>
        /// 订阅描述
        /// </summary>
        public string Descriptions { get; set; }

        public Guid PublisherId { get; set; }
    }
}
