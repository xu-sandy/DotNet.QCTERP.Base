using Qct.Infrastructure.MessageClient.ObjectModels;
using Qct.Infrastructure.Net.SocketClient;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Qct.Infrastructure.MessageClient.Implementations
{
    public class ExchangerClient : SocketClient
    {
        internal static readonly byte[] authRouteCode = new byte[] { 0x01, 0x00, 0x00, 0x01 };
        internal static readonly byte[] publishRouteCode = new byte[] { 0x01, 0x00, 0x00, 0x04 };
        internal static readonly byte[] subscribeRouteCode = new byte[] { 0x01, 0x00, 0x00, 0x02 };
        EventHandler ReConnected;
        public ExchangerClient(Action<object, EventArgs> reConnectedHandler) : base(new DefaultRouteProvider(4), typeof(ExchangerClient).Assembly)
        {
            ReConnected = new EventHandler(reConnectedHandler);
        }

        public override void Initialize()
        {
            base.Initialize();
            ConnectRemote();
            TryConnectRemote();
        }
        /// <summary>
        ///每10秒检查一次连接状态
        /// </summary>
        internal void TryConnectRemote()
        {
            var task = Task.Factory.StartNew(() =>
              {
                  Thread.Sleep(10000);
                  if (!IsConnected)
                      ConnectRemote();
                  TryConnectRemote();
              });

        }
        internal bool SendPublisherInformaction(PublisherInformaction publisher)
        {
            try
            {
                if (!IsConnected)
                {
                    return false;
                }
                var message = SendObjectToJsonStreamWithResponse(authRouteCode, publisher);
                SocketResult<string> result;
                if (message != null && message.TryReadFromJsonStream(out result))
                {
                    if (result != null && result.Code != "200")
                    {
                        return false;
                    }
                }
                else if (message == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        internal bool SendSubscribeItem(SubscribeItem item)
        {
            try
            {
                if (!IsConnected) return false;

                var message = SendObjectToJsonStreamWithResponse(subscribeRouteCode, item);
                SocketResult<string> result;
                if (message != null && message.TryReadFromJsonStream(out result))
                {
                    if (result != null && result.Code != "200")
                    {
                        return false;
                    }
                }
                else if (message == null)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        internal bool SendPublishItem(PublishItem item)
        {
            try
            {
                if (!IsConnected) return false;

                var message = SendObjectToJsonStreamWithResponse(publishRouteCode, item);
                SocketResult<string> result;
                if (message != null && message.TryReadFromJsonStream(out result))
                {
                    if (result != null && result.Code != "200")
                    {
                        return false;
                    }
                }
                else if (message == null)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        internal void ConnectRemote()
        {
            try
            {
                MessageQueueConfigurationSection config = MessageQueueConfigurationSection.GetConfig();
                var ips = Dns.GetHostAddresses(config.ExchangeServerIP);
                var task = ConnectAsync(new IPEndPoint(ips.FirstOrDefault(), config.ExchangeServerPort));
                task.Wait();
                if (IsConnected && ReConnected != null)
                {
                    ReConnected(this, new EventArgs());
                }
            }
            catch
            {
                //todo log
            }
        }
    }
}
