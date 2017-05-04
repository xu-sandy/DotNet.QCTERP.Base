using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using System;

namespace Qct.Infrastructure.Net.SocketServer
{
    /// <summary>
    /// 路由接收过滤器抽象
    /// </summary>
    /// <typeparam name="TRequestInfo">会话请求信息对象类</typeparam>
    public abstract class RouteReceiveFilter<TRequestInfo> : FixedHeaderReceiveFilter<TRequestInfo> where TRequestInfo : IRequestInfo
    {
        /// <summary>
        /// 初始化过滤器（默认会话信息头部为路由码+4位数据长度）
        /// </summary>
        /// <param name="routeProvider">路由提供程序</param>
        public RouteReceiveFilter(IRouteProvider routeProvider)
            : base(routeProvider.RouteLength + 4)
        {
            RouteProvider = routeProvider;
        }
        /// <summary>
        /// 路由提供程序
        /// </summary>
        public IRouteProvider RouteProvider { get; set; }
        /// <summary>
        /// 获取会话信息头部包含的消息体长度信息
        /// </summary>
        /// <param name="header"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return BitConverter.ToInt32(header, offset + RouteProvider.RouteLength);
        }
        /// <summary>
        /// 生成完整会话请求消息内容
        /// </summary>
        /// <param name="header">头部信息数据</param>
        /// <param name="bodyBuffer">缓冲区数据信息</param>
        /// <param name="offset">数据指针</param>
        /// <param name="length">数据长度</param>
        /// <returns>会话请求消息内容</returns>
        protected override TRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return LoadRequestInfo(header.Array.CloneRange(header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }
        /// <summary>
        /// 加载会话请求消息内容代理方法
        /// </summary>
        /// <param name="routeCode">路由码</param>
        /// <param name="bodyBuffer">缓冲区数据</param>
        /// <returns>会话请求消息内容</returns>
        public abstract TRequestInfo LoadRequestInfo(byte[] routeCode, byte[] bodyBuffer);

    }
}
