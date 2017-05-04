using Qct.Settings;

namespace Qct.IRepository
{
    public interface IPosElectronicBalanceSettingsRepository
    {
        /// <summary>
        /// 获取电子秤设置
        /// </summary>
        /// <param name="isNotFoundReturnDefault"></param>
        /// <returns></returns>
        ElectronicBalanceSetting Get(bool isNotFoundReturnDefault = false);

        /// <summary>
        /// 保存电子秤配置
        /// </summary>
        /// <param name="setting"></param>
        void Save(ElectronicBalanceSetting setting);
    }
}
