using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System.Net;

namespace Qct.Infrastructure.Net.SocketServer
{
    /// <summary>
    /// 默认接收过滤器工厂
    /// </summary>
    public class DefaultReceiveFilterFactory : IReceiveFilterFactory<SockectRequestMessage>
    {
        /// <summary>
        /// 创建过滤器
        /// </summary>
        /// <param name="appServer">socket服务器对象</param>
        /// <param name="appSession">socket会话对象</param>
        /// <param name="remoteEndPoint">远程节点信息</param>
        /// <returns></returns>
        public IReceiveFilter<SockectRequestMessage> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            var server = (SocketServer)appServer;
            IReceiveFilter<SockectRequestMessage> filter = new DefaultRouteReceiveFilter(server.RouteProvider);
            return filter;
        }
    }
}
