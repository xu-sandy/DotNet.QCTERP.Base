using SuperSocket.SocketBase.Command;
using System;

namespace Qct.Infrastructure.Net.SocketServer
{
    /// <summary>
    ///Socket命令基类
    /// </summary>
    public abstract class CommandBase : ICommand<SocketSession, SockectRequestMessage>
    {
        /// <summary>
        /// 初始化命令（使用默认路由提供程序）
        /// </summary>
        /// <param name="routeCode">路由编码</param>
        public CommandBase(params byte[] routeCode)
            : this(new DefaultRouteProvider(4), routeCode)
        {
        }
        /// <summary>
        /// 指定路由提供程序、路由码初始化命令
        /// </summary>
        /// <param name="routeProvider"></param>
        /// <param name="routeCode"></param>
        public CommandBase(IRouteProvider routeProvider, params byte[] routeCode)
        {
            if (routeCode == null || routeProvider.RouteLength != routeCode.Length)
            {
                throw new ArgumentException("路由与路由规则不匹配！");
            }
            RouteProvider = routeProvider;
            RouteCode = routeCode;
        }
        /// <summary>
        /// 执行命令由Execute完成代处理
        /// </summary>
        /// <param name="session">Socket会话</param>
        /// <param name="requestInfo">会话请求内容</param>
        public void ExecuteCommand(SocketSession session, SockectRequestMessage requestInfo)
        {
            Execute(session.SocketServer, session, requestInfo);
        }
        /// <summary>
        /// 代理处理命令
        /// </summary>
        /// <param name="server">socket服务器对象</param>
        /// <param name="session">socket会话对象</param>
        /// <param name="requestInfo">请求信息内容</param>
        public abstract void Execute(SocketServer server, SocketSession session, SockectRequestMessage requestInfo);
        /// <summary>
        /// 路由（由路由提供程序和路由码生成的路由信息）
        /// </summary>
        public string Name
        {
            get { return RouteProvider.GetRoute(RouteCode); }
        }
        /// <summary>
        /// 路由提供程序
        /// </summary>
        public IRouteProvider RouteProvider { get; private set; }
        /// <summary>
        /// 路由码
        /// </summary>
        public byte[] RouteCode { get; private set; }
    }
}
