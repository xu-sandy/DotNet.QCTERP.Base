using MongoDB.Bson;
using System;

namespace Qct.Infrastructure.MessageServer.Objects
{
    public class RemoteSubscribe
    {
        public ObjectId _id { get; set; }
        public string Group { get; set; }

        public Guid AgentId { get; set; }

    }
}
