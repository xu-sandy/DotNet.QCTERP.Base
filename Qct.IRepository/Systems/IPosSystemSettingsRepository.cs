using Qct.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.IRepository
{
    public interface IPosSystemSettingsRepository
    {
        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        SystemSettings Get();
        /// <summary>
        /// 保存系统配置
        /// </summary>
        /// <param name="settings">系统配置信息</param>
        void Save(SystemSettings settings);
    }
}
