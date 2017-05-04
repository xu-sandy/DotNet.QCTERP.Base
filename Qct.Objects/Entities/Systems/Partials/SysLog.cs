using Qct.Infrastructure.Data.EntityInterface;
using Qct.Infrastructure.Log;
using Qct.Objects.ValueObjects;
using System;

namespace Qct.Objects.Entities
{
    public partial class SysLog : BaseLogContent,CompanyEntity
    {
        public SysLog() { }
        public SysLog(int companyId, string uid)
        {
            CompanyId = companyId;
            UId = uid;
        }
        public SysLog Analysis(string msg, Exception ex, LogType type, LogModule module)
        {
            if (string.IsNullOrEmpty(msg) && ex != null)
            {
                Summary = FormatMessage(ex);
            }
            else
            {
                Summary = msg;
            }
            Type = (byte)type;
            ModuleName = Enum.GetName(module.GetType(), module);
            CreateDT = DateTime.Now;
            return this;
        }

        private void SetServerClient()
        {

        }
        #region 业务操作类型

        /// <summary>
        /// 写入新增日志
        /// </summary>
        /// <param name="msg">自定义信息</param>
        /// <param name="module">LogModule 日志模块</param>
        public SysLog WriteInsert(string msg, LogModule module)
        {
            return Analysis(msg, null, LogType.新增, module);
        }

        /// <summary>
        /// 写入修改日志
        /// </summary>
        /// <param name="msg">自定义信息</param>
        /// <param name="module">LogModule 日志模块</param>
        public SysLog WriteUpdate(string msg, LogModule module)
        {
            return Analysis(msg, null, LogType.修改, module);
        }

        /// <summary>
        /// 写入删除日志
        /// </summary>
        /// <param name="msg">自定义信息</param>
        /// <param name="module">LogModule 日志模块</param>
        public SysLog WriteDelete(string msg, LogModule module)
        {
            return Analysis(msg, null, LogType.删除, module);
        }

        #endregion

        #region 系统操作类型


        /// <summary>
        /// 写入登录日志
        /// </summary>
        /// <param name="msg">自定义信息</param>
        /// <param name="module">LogModule 日志模块</param>
        public SysLog WriteLogin(string msg, LogModule module)
        {
            return Analysis(msg, null, LogType.登录, module);
        }


        /// <summary>
        /// 写入退出日志
        /// </summary>
        /// <param name="msg">自定义信息</param>
        /// <param name="module">LogModule 日志模块</param>
        public SysLog WriteLogout(string msg, LogModule module)
        {
            return Analysis(msg, null, LogType.退出, module);
        }

        /// <summary>
        /// 写入异常日志
        /// </summary>
        /// <param name="ex">Exception 异常信息</param>
        /// <param name="module">LogModule 日志模块</param>
        public SysLog WriteError(Exception ex, LogModule module)
        {
            return Analysis(string.Empty, ex, LogType.异常, module);
        }

        /// <summary>
        /// 写入自定义信息的异常日志
        /// </summary>
        /// <param name="msg">自定义信息（优先级）</param>
        /// <param name="ex">Exception 异常信息</param>
        /// <param name="module">LogModule 日志模块</param>
        public SysLog WriteError(string msg, Exception ex, LogModule module)
        {
            return Analysis(msg, ex, LogType.异常, module);
        }

        #endregion
    }
}
