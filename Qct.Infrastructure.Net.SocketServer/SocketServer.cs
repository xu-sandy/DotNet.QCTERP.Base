using SuperSocket.SocketBase;
using System;

namespace Qct.Infrastructure.Net.SocketServer
{
    /// <summary>
    /// socket 服务器
    /// </summary>
    public class SocketServer : AppServer<SocketSession, SockectRequestMessage>
    {
        /// <summary>
        /// 使用指定的规则提供程序和默认的过滤器初始化服务器
        /// </summary>
        /// <param name="routeProvider">规则提供程序</param>
        public SocketServer(IRouteProvider routeProvider)
            : base(new DefaultReceiveFilterFactory())
        {
            RouteProvider = routeProvider;
        }
        /// <summary>
        /// 规则提供程序
        /// </summary>
        public IRouteProvider RouteProvider { get; private set; }
        /// <summary>
        /// 初始化服务器
        /// </summary>
        /// <param name="routeProvider">规则提供程序</param>
        /// <param name="port">端口</param>
        /// <returns>服务器对象</returns>
        public static SocketServer InitServer(IRouteProvider routeProvider, int port)
        {
            SocketServer appServer = new SocketServer(routeProvider);
            if (!appServer.Setup(port)) //Setup with listening port
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
