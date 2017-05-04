using Qct.Infrastructure.Data;
using Qct.Objects.Entities.Systems;
using Qct.Objects.ValueObjects;
using Qct.Objects.ValueObjects.Systems;
using System.Collections.Generic;

namespace Qct.IRepository
{
    public interface IDeviceRepository : IEFRepository<DeviceRegInfo>
    {
        /// <summary>
        /// 更改设备状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        OperateResult ChangeStatus(string ids, DeviceState state, string userId);
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        List<DeviceModel> GetList(out int recordCount);
        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        OperateResult SetMemo(int id, string memo);
        /// <summary>
        /// 根据查询条件查数据
        /// </summary>
        /// <param name="machineType"></param>
        /// <param name="store"></param>
        /// <param name="status"></param>
        /// <param name="keyword"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        List<DeviceModel> GetListByWhere(DeviceType machineType, string store, DeviceState status, string keyword, out int recordCount);

        /// <summary>
        ///获取门店下的所有设备编号
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        IEnumerable<string> GetMachineSns(int companyId, string storeId);

        DeviceRegInfo Get(int companyId, string storeId, string machineSn);


    }
}
