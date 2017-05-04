using Qct.Infrastructure.MessageClient.ObjectModels;
using Qct.Infrastructure.MessageServer.Exceptions;
using Qct.Infrastructure.MessageServer.Implementations;
using Qct.Infrastructure.Net.SocketServer;
using System;

namespace Qct.Infrastructure.MessageServer.SocketCommands
{
    public class AuthenticationCommand : CommandBase
    {
        public AuthenticationCommand() : base(0x01, 0x00, 0x00, 0x01) { }

        public override void Execute(SocketServer server, SocketSession session, SockectRequestMessage requestInfo)
        {
            try
            {
                PublisherInformaction publisher;
                if (requestInfo.TryReadFromJsonStream(out publisher))
                {
                    var _MQMServer = (MQMServer)server;
                    if (_MQMServer.MessageQueueAgent.Authentication(new ClientIndetity() { IsWebSite = publisher.IsWebSite, PublisherId = publisher.PublisherId, SessionId = session.SessionID, WebSiteHost = publisher.WebSiteHost }, publisher.MQMToken))
                    {
                        var result = SocketResult<string>.Create(message: "欢迎登录消息中间件！");
                        session.SendObjectToJsonStream(RouteCode, result);
                    }
                }
            }
            catch (NotAuthenticationException)
            {
                var result = SocketResult<string>.Create(code: "501", message: "授权登陆失败！");
                session.SendObjectToJsonStream(RouteCode, result);
            }
            catch (Exception ex)
            {
                var result = SocketResult<string>.Create(code: "600", message: string.Format("授权登陆过程中发生无法预知错误，{0}！", ex.Message));
                session.SendObjectToJsonStream(RouteCode, result);
            }
        }

    }
}

