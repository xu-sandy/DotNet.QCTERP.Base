using MongoDB.Bson;
using System;

namespace Qct.Infrastructure.MessageServer.Objects
{
    public class RemoteSubscriptionCallbackFailureRecord
    {
        public ObjectId _id { get; set; }
        public string Content { get; set; }

        public Guid PublisherId { get; set; }

    }
}
