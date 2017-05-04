using MongoDB.Bson;
using Qct.Infrastructure.MessageClient.ObjectModels;
using System;

namespace Qct.Infrastructure.MessageServer.Objects
{
    public class RemotePushFailureRecord
    {
        public ObjectId _id { get; set; }
        public Guid AgentId { get; set; }

        public PublishItem Content { get; set; }
    }
}
