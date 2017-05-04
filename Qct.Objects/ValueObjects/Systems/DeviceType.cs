using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.ValueObjects.Systems
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public enum DeviceType:short
    {
        /// <summary>
        /// PC或者大POS机
        /// </summary>
        PC=1,
        /// <summary>
        /// 平板
        /// </summary>
        Pad=2,
        /// <summary>
        /// 手机
        /// </summary>
        Moblie=3,
    }
}
