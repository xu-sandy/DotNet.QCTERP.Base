using log4net;
using System;
using System.Threading.Tasks;

namespace Qct.Infrastructure.MessageServer.Implementations
{
    public class SockectClientChannel : IClientChannel
    {
        /// <summary>
        /// Socket推送路由码
        /// </summary>
        private static readonly byte[] pulishRouteCode = new byte[] { 0x01, 0x00, 0x00, 0x05 };
        public SockectClientChannel(MQMServer server, string sessionId, ILog logger)
        {
            Server = server;
            SessionId = sessionId;
            Logger = logger;
        }
        /// <summary>
        /// 消息中间件服务器对象
        /// </summary>
        public MQMServer Server { get; private set; }
        /// <summary>
        /// socket会话ID
        /// </summary>
        public string SessionId { get; private set; }
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILog Logger { get; private set; }

        public bool SendMessage(string message)
        {
            try
            {
                var session = Server.GetSessionByID(SessionId);
                if (session != null && session.Connected)
                {
                    session.SendTextToStream(pulishRouteCode, message);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew((e) =>
                {
                    var exception = e as Exception;
                    if (exception != null && Logger != null)
                    {
                        Logger.Error("Socket客户端消息管道推送消息失败！", exception);
                    }
                }, ex);
            }
            return false;
        }
    }
}
