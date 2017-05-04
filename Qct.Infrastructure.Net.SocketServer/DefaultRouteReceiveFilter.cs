namespace Qct.Infrastructure.Net.SocketServer
{
    /// <summary>
    /// 默认路由提供程序路由码过滤器
    /// </summary>
    public class DefaultRouteReceiveFilter : RouteReceiveFilter<SockectRequestMessage>
    {
        /// <summary>
        /// 初始化过滤器
        /// </summary>
        /// <param name="routeProvider">路由提供程序</param>
        public DefaultRouteReceiveFilter(IRouteProvider routeProvider)
            : base(routeProvider)
        {

        }
        /// <summary>
        /// 加载路由信息
        /// </summary>
        /// <param name="routeCode">路由码</param>
        /// <param name="bodyBuffer">会话信息缓冲数据</param>
        /// <returns>socket请求信息</returns>
        public override SockectRequestMessage LoadRequestInfo(byte[] routeCode, byte[] bodyBuffer)
        {
            var route = RouteProvider.GetRoute(routeCode);
            SockectRequestMessage msg = new SockectRequestMessage(route, bodyBuffer);
            return msg;
        }
    }
}
