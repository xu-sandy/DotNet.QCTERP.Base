using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.MessageServer.Implementations
{
    /// <summary>
    /// 消息中间件配置
    /// </summary>
    public class MQMConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 获取配置实例
        /// </summary>
        /// <returns></returns>
        public static MQMConfigurationSection GetConfig()
        {
            MQMConfigurationSection section = (MQMConfigurationSection)ConfigurationManager.GetSection("MessageServer");
            return section;
        }
        /// <summary>
        /// 消息中间服务端口
        /// </summary>
        [ConfigurationProperty("Port", DefaultValue = 3033, IsRequired = false)]
        public int Port
        {
            get
            {
                return (int)base["Port"];
            }
            set
            {
                base["Port"] = value;
            }
        }

        [ConfigurationProperty("Password", DefaultValue = "admin", IsRequired = false)]
        public string Password
        {
            get
            {
                return (string)base["Password"];
            }
            set
            {
                base["Password"] = value;
            }
        }

        [ConfigurationProperty("MessageQueueConnectionString", DefaultValue = "127.0.0.1:6379,password=pharos@2016", IsRequired = false)]
        public string MessageQueueConnectionString
        {
            get
            {
                return (string)base["MessageQueueConnectionString"];
            }
            set
            {
                base["MessageQueueConnectionString"] = value;
            }
        }

        [ConfigurationProperty("StorageConnectionString", DefaultValue = "mongodb://localhost:27017/mqmdb", IsRequired = false)]
        public string StorageConnectionString
        {
            get
            {
                return (string)base["StorageConnectionString"];
            }
            set
            {
                base["StorageConnectionString"] = value;
            }
        }

        [ConfigurationProperty("RetryRemotePushFailureInterval", DefaultValue = 300000, IsRequired = false)]
        public int RetryRemotePushFailureInterval
        {
            get
            {
                return (int)base["RetryRemotePushFailureInterval"];
            }
            set
            {
                base["RetryRemotePushFailureInterval"] = value;
            }
        }
    }
}
