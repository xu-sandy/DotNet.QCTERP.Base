using Qct.Objects.ValueObjects;

namespace Qct.Settings
{
    /// <summary>
    /// 收银台信息
    /// </summary>
    public class POSDeviceInformation
    {

        public POSDeviceInformation() { }

        public POSDeviceInformation(StoreInformation storeInfo)
        {
            CompanyId = storeInfo.CompanyId;
            StoreId = storeInfo.StoreId;
            StoreName = storeInfo.StoreName;
            CompanyName = storeInfo.CompanyName;
            CompanyShorterName = storeInfo.CompanyShorterName;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 门店Id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 设备后台编号
        /// </summary>
        public string MachineSn { get; set; }
        /// <summary>
        /// 设备唯一标识
        /// </summary>
        public string DeviceSn { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string CompanyShorterName { get; set; }

        /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <param name="machineSn"></param>
        /// <param name="deviceSn"></param>
        public void SetMachine(string machineSn, string deviceSn)
        {
            MachineSn = machineSn;
            DeviceSn = deviceSn;
        }
    }
}
