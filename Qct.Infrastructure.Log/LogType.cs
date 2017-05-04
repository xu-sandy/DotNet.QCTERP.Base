using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.Log
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 登录
        /// </summary>
        登录 = 1,
        /// <summary>
        /// 退出
        /// </summary>
        退出 = 2,
        /// <summary>
        /// 异常
        /// </summary>
        异常 = 3,
        /// <summary>
        /// 新增
        /// </summary>
        新增 = 4,
        /// <summary>
        /// 修改
        /// </summary>
        修改 = 5,
        /// <summary>
        /// 删除
        /// </summary>
        删除 = 6,
        /// <summary>
        /// 调试
        /// </summary>
        调试 = 7,
        /// <summary>
        /// 演练
        /// </summary>
        演练 = 8,
        /// <summary>
        /// 其他
        /// </summary>
        其他 = 10
    }
}
