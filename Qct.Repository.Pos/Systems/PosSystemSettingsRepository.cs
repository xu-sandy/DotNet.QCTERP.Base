using Qct.IRepository;
using Qct.Settings;
using System.Xml.Linq;
using Qct.IRepository.Exceptions;
using System.IO;
using Qct.Repository.Pos.Common;
using System;
using Qct.Infrastructure.Log;
using Qct.Objects.ValueObjects;

namespace Qct.Repository
{
    /// <summary>
    /// 系统设置仓储
    /// </summary>
    public class PosSystemSettingsRepository : IPosSystemSettingsRepository
    {
        /// <summary>
        /// 配置文件存放路径
        /// </summary>
        private readonly string ConfigFilePath = SettingsPathHelper.GetOrCreateSettingsPath();
        /// <summary>
        /// 系统配置文件名称
        /// </summary>
        private const string SystemSettingFileName = "System.Config";

        private static SystemSettings settings;
        /// <summary>
        /// 获取系统设置
        /// </summary>
        /// <returns>系统设置</returns>
        public SystemSettings Get()
        {
            if (settings != null)
                return settings;
            var fileName = Path.Combine(ConfigFilePath, SystemSettingFileName);
            try
            {
                if (File.Exists(fileName))
                {
                    var doc = XDocument.Load(fileName);
                    var root = doc.Element("SystemSettings");
                    var messageServerConfig = root.Element("MessageServer");
                    var remoteServerConfig = root.Element("RemoteServer");
                    var result = new SystemSettings()
                    {
                        EnableRemoteService = bool.Parse(root.Attribute("EnableRemoteService").Value),
                        MessageServer = new ServerConfiguration()
                        {
                            Schema = messageServerConfig.Attribute("Schema").Value,
                            Host = messageServerConfig.Attribute("Host").Value,
                            Port = int.Parse(messageServerConfig.Attribute("Port").Value)
                        },
                        RemoteServer = new ServerConfiguration()
                        {
                            Schema = remoteServerConfig.Attribute("Schema").Value,
                            Host = remoteServerConfig.Attribute("Host").Value,
                            Port = int.Parse(remoteServerConfig.Attribute("Port").Value)
                        }
                    };
                    settings = result;
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                //LoggerFactory.Create(ERPModule.POSClient.GetModuleName(), LoggerType.Log4net).Warn("获取系统设置失败！", ex);
                throw new SettingException("读取系统设置时出错，请检查配置是否正确！");
            }
        }
        /// <summary>
        /// 保存系统设置
        /// </summary>
        /// <param name="settings">系统设置</param>
        public void Save(SystemSettings settings)
        {
            var fileName = Path.Combine(ConfigFilePath, SystemSettingFileName);

            XElement root = new XElement("SystemSettings");
            root.SetAttributeValue(XName.Get("EnableRemoteService"), settings.EnableRemoteService);
            XElement messageServerConfig = new XElement("MessageServer");
            messageServerConfig.SetAttributeValue(XName.Get("Schema"), settings.MessageServer.Schema);
            messageServerConfig.SetAttributeValue(XName.Get("Host"), settings.MessageServer.Host);
            messageServerConfig.SetAttributeValue(XName.Get("Port"), settings.MessageServer.Port);
            var remoteServerConfig = new XElement("RemoteServer");
            remoteServerConfig.SetAttributeValue(XName.Get("Schema"), settings.RemoteServer.Schema);
            remoteServerConfig.SetAttributeValue(XName.Get("Host"), settings.RemoteServer.Host);
            remoteServerConfig.SetAttributeValue(XName.Get("Port"), settings.RemoteServer.Port);
            root.Add(messageServerConfig, remoteServerConfig);
            root.Save(fileName);
        }
    }
}
