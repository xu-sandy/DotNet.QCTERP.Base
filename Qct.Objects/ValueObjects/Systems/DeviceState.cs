using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.ValueObjects.Systems
{
    /// <summary>
    /// 设备状态
    /// </summary>
    public enum DeviceState : short
    {
        /// <summary>
        /// 禁用
        /// </summary>
        Disable=0,
        /// <summary>
        /// 启用
        /// </summary>
        Enable = 1
    }
}
