using Qct.Objects.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.ISevices.Systems
{
    /// <summary>
    /// 设备服务
    /// </summary>
    public interface IDeviceService
    {
        /// <summary>
        /// 获取设备安全码
        /// </summary>
        /// <param name="storeInfo">门店信息</param>
        /// <returns>安全码</returns>
        string GetSecurityCode(StoreInformation storeInfo);
        /// <summary>
        /// 创建设备编号
        /// </summary>
        /// <param name="machineSns">已经存在的设备编号</param>
        /// <returns></returns>
        string CreateMachineSn(IEnumerable<string> machineSns, int companyId, string storeId);
        /// <summary>
        /// 解密设备安全码
        /// </summary>
        /// <param name="certificateContent"></param>
        /// <returns></returns>
        StoreInformation DecryptSecurityCode(string certificateContent);
    }
}
