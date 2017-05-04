using Qct.Infrastructure.MessageClient.ObjectModels;
using Qct.Infrastructure.MessageServer.Exceptions;
using Qct.Infrastructure.MessageServer.Implementations;
using Qct.Infrastructure.Net.SocketServer;
using System;

namespace Qct.Infrastructure.MessageServer.SocketCommands
{
    public class SubscribeCommad : CommandBase
    {
        public SubscribeCommad() : base(0x01, 0x00, 0x00, 0x02) { }
        public override void Execute(SocketServer server, SocketSession session, SockectRequestMessage requestInfo)
        {
            try
            {
                SubscribeItem subscribeItem;
                if (requestInfo.TryReadFromJsonStream(out subscribeItem))
                {
                    var _MQMServer = (MQMServer)server;
                    _MQMServer.MessageQueueAgent.Subscribe(subscribeItem.PublisherId, subscribeItem);
                    var result = SocketResult<string>.Create(message: "订阅主题成功！");
                    session.SendObjectToJsonStream(RouteCode, result);
                }
            }
            catch (NotAuthenticationException)
            {
                var result = SocketResult<string>.Create(code: "500", message: "未经授权的操作！");
                session.SendObjectToJsonStream(RouteCode, result);
            }
            catch (Exception ex)
            {
                var result = SocketResult<string>.Create(code: "600", message: string.Format("订阅主题过程中发生无法预知错误，{0}！", ex.Message));
                session.SendObjectToJsonStream(RouteCode, result);
            }
        }
    }
}
