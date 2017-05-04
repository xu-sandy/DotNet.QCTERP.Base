using System;
using System.Collections.Generic;
using System.Linq;

namespace Qct.Infrastructure.MessageServer.Implementations
{

    public class RedisMessageQueueAdapter : IMessageQueueAdapter
    {
        public void Publish(string descriptions, string content)
        {
            RedisManager.Publish(descriptions, content);
        }

        public void Subscribe(string group, RemotePublish callback)
        {
            RedisManager.Subscribe(group + "*", (key, value) =>
            {
                callback(key, value);
            });
        }

        public void UnSubscribe(string group)
        {
            RedisManager.UnSubscribe(group + "*");
        }
    }
}
