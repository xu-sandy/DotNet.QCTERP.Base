using System.Configuration;

namespace Qct.Infrastructure.MessageClient
{
    public class MessageQueueConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 获取配置实例
        /// </summary>
        /// <returns></returns>
        public static MessageQueueConfigurationSection GetConfig()
        {
            MessageQueueConfigurationSection section = (MessageQueueConfigurationSection)ConfigurationManager.GetSection("MessageQueue");
            return section;
        }
        /// <summary>
        /// 消息中间服务端口
        /// </summary>
        [ConfigurationProperty("EnaleExchange", DefaultValue = false, IsRequired = false)]
        public bool EnaleExchange
        {
            get
            {
                return (bool)base["EnaleExchange"];
            }
            set
            {
                base["EnaleExchange"] = value;
            }
        }

        /// <summary>
        /// 消息中间服务端口
        /// </summary>
        [ConfigurationProperty("ExchangeServerIP", DefaultValue = "127.0.0.1", IsRequired = false)]
        public string ExchangeServerIP
        {
            get
            {
                return (string)base["ExchangeServerIP"];
            }
            set
            {
                base["ExchangeServerIP"] = value;
            }
        }
        /// <summary>
        /// 消息中间服务端口
        /// </summary>
        [ConfigurationProperty("ExchangeServerPort", DefaultValue = 3033, IsRequired = false)]
        public int ExchangeServerPort
        {
            get
            {
                return (int)base["ExchangeServerPort"];
            }
            set
            {
                base["ExchangeServerPort"] = value;
            }
        }

        [ConfigurationProperty("ExchangeServerPassword", DefaultValue = "admin", IsRequired = false)]
        public string MQMPassword
        {
            get
            {
                return (string)base["ExchangeServerPassword"];
            }
            set
            {
                base["ExchangeServerPassword"] = value;
            }
        }

        [ConfigurationProperty("WebSiteHost", DefaultValue = "http://localhost/", IsRequired = false)]
        public string WebSiteHost
        {
            get
            {
                return (string)base["WebSiteHost"];
            }
            set
            {
                base["WebSiteHost"] = value;
            }
        }
    }
}
