using log4net;
using Qct.Infrastructure.Net.Http.RestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.MessageServer.Implementations
{
    public class HttpClientChannel : IClientChannel
    {
        /// <summary>
        /// Web站点Http推送路径
        /// </summary>
        private static readonly string pulishWebRouteCode = "Greenery/Event";
        public HttpClientChannel(string host, ILog logger)
        {
            Host = host;
            Logger = logger;
        }
        /// <summary>
        /// Web 站点地址（根地址）
        /// </summary>
        public string Host { get; private set; }
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILog Logger { get; private set; }

        public bool SendMessage(string message)
        {
            try
            {
                var url = new Uri(new UriBuilder(Host).Uri, pulishWebRouteCode);
                var settings = new RequestSetting(url) { Method = "POST" };
                var request = new RestRequest<string>(settings);
                request.ExecuteWithString();
                return true;
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
