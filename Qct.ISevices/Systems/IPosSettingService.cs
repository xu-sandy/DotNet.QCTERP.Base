using Qct.Settings;

namespace Qct.IServices
{
    /// <summary>
    /// Pos设置服务接口
    /// </summary>
    public interface IPosSettingService
    {
        /// <summary>
        /// 设备注册
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        POSDeviceInformation DeviceRegister(string certificate);

    }
}
