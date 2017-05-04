namespace Qct.Infrastructure.Net.SocketServer
{
    /// <summary>
    /// 路由提供程序接口
    /// </summary>
    public interface IRouteProvider
    {
        /// <summary>
        /// 获取路由
        /// </summary>
        /// <param name="routeCode">路由码</param>
        /// <returns>路由信息</returns>
        string GetRoute(byte[] routeCode);
        /// <summary>
        /// 路由长度
        /// </summary>
        int RouteLength { get; }
    }
}
