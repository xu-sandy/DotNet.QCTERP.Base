using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Settings
{
    public class SystemSettings
    {
        /// <summary>
        /// 开机自启动
        /// </summary>
        public bool AutoRunInWindowStart { get; set; }

        /// <summary>
        /// 是否启用远程服务，未开启远程服务，所有服务将只运行在门店备份环境中
        /// </summary>
        public bool EnableRemoteService { get; set; }
        /// <summary>
        /// 远程服务器
        /// </summary>
        public ServerConfiguration RemoteServer { get; set; }

        /// <summary>
        /// 消息服务器
        /// </summary>
        public ServerConfiguration MessageServer { get; set; }
        /// <summary>
        /// 获取当前服务接口的服务器配置
        /// </summary>
        /// <returns></returns>
        public ServerConfiguration GetCurrentServer()
        {
            return RemoteServer;
        }

    }
}
