using StackExchange.Redis;
using System;

namespace Qct.Infrastructure.MessageServer.Implementations
{
    public class RedisManager
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>


        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(MQMConfigurationSection.GetConfig().MessageQueueConnectionString);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public static void Subscribe(string title, Action<RedisChannel, string> handler)
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.Subscribe(title, (channel, msg) => { handler(channel, msg); });
        }


        public static void UnSubscribe(string title)
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.Unsubscribe(new RedisChannel(title, RedisChannel.PatternMode.Auto));
        }
        public static void Publish(string title, string info)
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.Publish(title, info);
        }
    }
}
