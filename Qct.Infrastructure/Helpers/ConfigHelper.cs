using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Xml.Linq;

namespace Qct.Infrastructure.Helpers
{
    public class ConfigHelper
    {


        #region 获取Web.Config配置指定的节点名称

        /// <summary>
        /// 获取Web.Config配置指定的节点名称
        /// </summary>
        /// <param name="elementName">AppSettings中的配置节名称</param>
        /// <param name="propName">属性名称</param>
        public static string GetAppSettings(string elementName, string attributeName = null)
        {
            if (ConfigurationManager.AppSettings[elementName] != null)
            {
                return ConfigurationManager.AppSettings[elementName].ToString();
            }
            var val = "";
            try
            {
                var filepath = new ConfigHelper().WebConfig.FilePath;
                XDocument doc = XDocument.Load(filepath);
                XElement ele = null;
                GetElement(doc.Root, elementName, ref ele);
                if (ele != null) val = ele.Attribute(attributeName).Value;
            }
            catch { }
            return val;
        }
        static void GetElement(XElement ele, string elementName, ref XElement rtnEl)
        {
            foreach (var el in ele.Elements())
            {
                if (string.Equals(el.Name.LocalName, elementName, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    rtnEl = el;
                    break;
                }
                else
                    GetElement(el, elementName, ref rtnEl);
            }
        }


        #endregion

        private Configuration WebConfig { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ConfigHelper()
            : this(System.Web.HttpContext.Current.Request.ApplicationPath)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFilePath">Config 配置文件路径</param>
        public ConfigHelper(string configFilePath)
        {
            this.WebConfig = WebConfigurationManager.OpenWebConfiguration(configFilePath);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            this.WebConfig.Save();
            this.WebConfig = null;
        }

        #region appSettings节点

        /// <summary>
        /// 设置appSettings节点
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public bool SetAppSettings(string key, string value)
        {
            try
            {
                //AppSettingsSection setting = (AppSettingsSection)this.WebConfig.GetSection("appSettings");
                var file = System.AppDomain.CurrentDomain.BaseDirectory + "settings.config";
                XDocument doc = XDocument.Load(file);
                var ele = (from x in doc.Element("appSettings").Elements("add") where x.Attribute("key").Value == key select x).FirstOrDefault();
                if (ele == null)
                {
                    var e = new XElement("add");
                    e.SetAttributeValue("key", key);
                    e.SetAttributeValue("value", value);
                    doc.Element("appSettings").Add(e);
                }
                else
                {
                    ele.SetAttributeValue("value", value);
                }
                //this.WebConfig.Save();注释会被清空
                doc.Save(file);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 移除appSettings节点
        /// </summary>
        /// <param name="key">键</param>
        public void RemoveAppSettings(string key)
        {
            AppSettingsSection setting = (AppSettingsSection)this.WebConfig.GetSection("appSettings");

            if (setting.Settings[key] != null)
            {
                setting.Settings.Remove(key);
            }
        }

        #endregion

    }
}
