using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Settings
{
    public class ServerConfiguration
    {
        public ServerConfiguration()
        {
            Schema = "http";
        }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 请求模式
        /// </summary>
        public string Schema { get; set; }
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        public override string ToString()
        {
            return string.Format("{0}://{1}:{2}/", Schema, Host, Port);
        }
        public bool Verfy()
        {
            var result = false;
            switch (Schema.ToLower())
            {
                case "tcp":
                case "http":
                case "https":
                    result = !string.IsNullOrWhiteSpace(Host);
                    if (result)
                    {
                        result = Port > 0;
                    }
                    break;

            }
            return result;
        }
    }
}
