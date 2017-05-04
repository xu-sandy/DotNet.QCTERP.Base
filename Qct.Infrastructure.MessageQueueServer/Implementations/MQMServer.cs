using Qct.Infrastructure.Net.SocketServer;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.MessageServer.Implementations
{
    public class MQMServer : SocketServer
    {
        public MQMServer() : base(new DefaultRouteProvider(4))
        {
            MessageQueueAgent = new MessageQueueAgent(this);
        }
        internal IMessageQueueAgent MessageQueueAgent { get; private set; }
        public static MQMServer InitServer()
        {
            return InitServer(MQMConfigurationSection.GetConfig().Port);
        }
        /// <summary>
        /// 通过端口初始化服务器
        /// </summary>
        /// <param name="port">端口</param>
        /// <returns>消息中间件服务器对象</returns>
        public static MQMServer InitServer(int port = 3033)
        {
            MQMServer appServer = new MQMServer();
            var listenners = new List<IListenerConfig>();
            listenners.Add(new ListenerConfig() { Ip = "IPv6Any", Port = port });
            listenners.Add(new ListenerConfig() { Ip = "Any", Port = port });

            ServerConfig config = new ServerConfig()
            {
                Listeners = listenners
            };
            if (!appServer.Setup(config)) //Setup with listening port
            {
                throw new Exception("Failed to setup!");
            }
            if (!appServer.Start())
            {
                throw new Exception("Failed to start!");
            }
            return appServer;
        }
    }
}
