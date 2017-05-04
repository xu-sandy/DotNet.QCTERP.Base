using log4net;
using Qct.Infrastructure.MessageClient.ObjectModels;

namespace Qct.Infrastructure.MessageServer.Implementations
{
    public class SendToClientChannel
    {
        public SendToClientChannel(MQMServer server, ILog logger)
        {
            Server = server;
            Logger = logger;
        }
        /// <summary>
        /// 消息中间件服务器对象
        /// </summary>
        public MQMServer Server { get; private set; }
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILog Logger { get; private set; }

        public bool SendMesssage(ClientIndetity indetity, string message)
        {
            try
            {
                if (message.Contains(indetity.PublisherId.ToString())) return true;

                IClientChannel clentChannel = new SockectClientChannel(Server, indetity.SessionId, Logger);
                if (clentChannel.SendMessage(message))
                {
                    return true;
                }
                if (indetity.IsWebSite && !string.IsNullOrEmpty(indetity.WebSiteHost))
                {
                    clentChannel = new HttpClientChannel(indetity.WebSiteHost, Logger);
                    if (clentChannel.SendMessage(message))
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
