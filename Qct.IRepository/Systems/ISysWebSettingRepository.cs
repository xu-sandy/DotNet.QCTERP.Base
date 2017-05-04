using Qct.Infrastructure.Data;
using Qct.Objects.Entities;

namespace Qct.IRepository
{
    public interface ISysWebSettingRepository: IEFRepository<SysWebSetting>
    {
        string GetLogo();
        SysWebSetting GetWebSetting();
        void SaveWebSetting(SysWebSetting model);
    }
}