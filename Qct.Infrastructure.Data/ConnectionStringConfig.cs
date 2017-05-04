/*
 * 开发人员：余雄文
 * 日期：2017-02-08
 */
using Qct.Infrastructure.Exceptions;
using Qct.Infrastructure.Security;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace Qct.Infrastructure.Data.Configuration
{
    /// <summary>
    /// 连接字符串读取工具类（含解密）
    /// </summary>
    public class ConnectionStringConfig
    {
        private readonly static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
        private readonly static string DESKey = "QCT";
        /// <summary>
        /// 缓存连接字符串
        /// </summary>
        private readonly static Dictionary<string, string> ConnectionStringsCache = new Dictionary<string, string>();
        /// <summary>
        /// 从配置文件中读取连接字符串
        /// </summary>
        /// <param name="name">AppSettings或者ConnectionString的name</param>
        /// <returns>连接字符串</returns>
        private static string ReadConnectionStringFromConfigFile(string name)
        {
            var connectionString = string.Empty;
            if (ConfigurationManager.ConnectionStrings[name] != null)
            {
                connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            if (ConfigurationManager.AppSettings[name] != null)
            {
                connectionString = ConfigurationManager.AppSettings[name];
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new DataException(string.Format("未找到连接字符串【{0}】!", name));
            }
            var connEncrypt = true;
            var connEncryptText = ConfigurationManager.AppSettings["ConnEncrypt"];

            if (!string.IsNullOrWhiteSpace(connEncryptText) && bool.TryParse(connEncryptText, out connEncrypt) && connEncrypt)
            {
                connectionString = DES.DESDecryptHexStringWithKeyIVToMd5Hex(connectionString, DESKey, DESKey);
            }
            return connectionString;
        }
        /// <summary>
        ///根据AppSettings或者ConnectionString的name 查找连接字符串
        /// </summary>
        /// <param name="name">AppSettings或者ConnectionString的name </param>
        /// <returns>连接字符串</returns>
        public static string GetConnectionString(string name = "ConnectionString")
        {
            rwl.EnterReadLock();
            try
            {
                if (ConnectionStringsCache.ContainsKey(name))
                {
                    return ConnectionStringsCache[name];
                }
            }
            finally
            {
                rwl.ExitReadLock();
            }
            try
            {
                rwl.EnterWriteLock();
                var connectionString = ReadConnectionStringFromConfigFile(name);
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new DataException("读取连接字符串失败，连接字符串无法解密或者未找到连接字符串！");
                ConnectionStringsCache[name] = connectionString;
                return connectionString;
            }
            finally
            {
                rwl.ExitWriteLock();
            }
        }
    }
}
