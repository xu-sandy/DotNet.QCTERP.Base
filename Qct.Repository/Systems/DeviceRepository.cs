using System;
using System.Collections.Generic;
using System.Linq;
using Qct.Objects.ValueObjects;
using Qct.IRepository;
using Qct.Objects.Entities.Systems;
using Qct.Objects.ValueObjects.Systems;

namespace Qct.Repository
{
    public class DeviceRepository : BaseEFRepository<DeviceRegInfo>, IDeviceRepository
    {

        public OperateResult ChangeStatus(string ids, DeviceState state, string userId)
        {
            try
            {
                var sid = ids.Split(',').Select(o => int.Parse(o)).ToList();
                var list = GetEntities().Where(o => sid.Contains(o.Id)).ToList();
                if (state == DeviceState.Enable)
                {
                    var stores = list.Select(o => o.StoreId).ToList();
                    var machines = list.Select(o => o.MachineSN).ToList();
                    var devices = GetEntities().Where(o => stores.Contains(o.StoreId) && machines.Contains(o.MachineSN) && !sid.Contains(o.Id) && o.State ==  DeviceState.Enable).ToList();
                    devices.ForEach(o =>
                    {
                        o.State = 0;
                    });
                }
                list.ForEach(o =>
                {
                    o.State = state;
                    o.AuditorUID = userId;
                });
                SaveChanges();
                return OperateResult.Success("操作成功！");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<string> GetMachineSns(int companyId, string storeId)
        {
            SetCompanyId(companyId);
           return GetEntities().Where(o => o.CompanyId == companyId && o.StoreId == storeId).Select(o => o.MachineSN).ToList();
        }

        public List<DeviceModel> GetList(out int recordCount)
        {
            var query = (from device in GetEntities()
                         select new DeviceModel()
                         {
                             StoreId = device.StoreId,
                             Id = device.Id,
                             DeviceSN = device.DeviceSN,
                             MachineSN = device.MachineSN,
                             Store = device.Store.Title,
                             CreateDT = device.CreateDT,
                             State = device.State,
                             Auditor = device.Auditor.FullName,
                             Memo = device.Memo
                         }).ToList();
            recordCount = query.Count;
            return query.OrderBy(a => a.CreateDT).ToList();
        }

        public List<DeviceModel> GetListByWhere(DeviceType machineType, string store, DeviceState status, string keyword, out int recordCount)
        {
            var query = GetReadOnlyEntities();
            if ((short)machineType != -1)
            {
                query = query.Where(o => o.Type == machineType);
            }
            if (!string.IsNullOrEmpty(store))
            {
                query = query.Where(o => o.StoreId == store);
            }
            if ((short)status != -1)
            {
                query = query.Where(o => o.State == status);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(o => o.DeviceSN.Contains(keyword) || o.Memo.Contains(keyword));
            }
            var result = (from device in query
                          select new DeviceModel()
                          {
                              StoreId = device.StoreId,
                              Id = device.Id,
                              Type = device.Type,
                              DeviceSN = device.DeviceSN,
                              MachineSN = device.MachineSN,
                              Store = device.Store.Title,
                              CreateDT = device.CreateDT,
                              State = device.State,
                              Auditor = device.Auditor.FullName,
                              Memo = device.Memo
                          }).ToList();
            recordCount = result.Count;
            return result;
        }

        public OperateResult SetMemo(int id, string memo)
        {
            try
            {
                var list = GetEntities().FirstOrDefault(o => o.Id == id);
                list.Memo = memo;
                SaveChanges();
                return OperateResult.Success("操作成功！");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DeviceRegInfo Get(int companyId, string storeId, string machineSn)
        {
            SetCompanyId(companyId);
           return GetReadOnlyEntities().FirstOrDefault(o => o.CompanyId == companyId && o.StoreId == storeId && o.MachineSN == machineSn);
        }
    }
}
