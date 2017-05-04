using System;

namespace Qct.Infrastructure.Net.SocketServer
{
    /// <summary>
    /// 默认路由规则提供程序
    /// </summary>
    public class DefaultRouteProvider : IRouteProvider
    {
        /// <summary>
        /// 初始化提供程序
        /// </summary>
        /// <param name="routeLength">路由码长度</param>
        public DefaultRouteProvider(int routeLength)
        {
            RouteLength = routeLength;
        }
        /// <summary>
        /// 获取路由
        /// </summary>
        /// <param name="routeCode">路由码</param>
        /// <returns>路由信息</returns>
        public string GetRoute(byte[] routeCode)
        {
            return BitConverter.ToString(routeCode);
        }
        /// <summary>
        /// 路由码长度
        /// </summary>
        public int RouteLength { get; private set; }
    }
}
