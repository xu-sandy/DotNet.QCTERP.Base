using System;
using System.IO;
using System.Security.AccessControl;

namespace Qct.Repository.Pos.Common
{
    public static class SettingsPathHelper
    {
        /// <summary>
        /// 获取系统配置文件夹路径，没有则创建并设置文件夹权限
        /// </summary>
        /// <returns></returns>
        public static string GetOrCreateSettingsPath()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");
            if (!Directory.Exists(configFilePath))
            {
                DirectorySecurity securityRules = new DirectorySecurity();
                securityRules.AddAccessRule(new FileSystemAccessRule(string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName), FileSystemRights.FullControl, AccessControlType.Allow));
                Directory.CreateDirectory(configFilePath, securityRules);
            }
            return configFilePath;
        }
    }
}
