using Qct.Infrastructure.Data;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
namespace Qct.IRepository
{
    public interface IWarehouseRepository: IEFRepository<Warehouse>
    {
        DataTable FindPageList(NameValueCollection nvl);
        List<Warehouse> GetAdminList(bool isAll = false);
        List<Warehouse> GetAdminList(string sid);
        List<Warehouse> GetList(bool isAll = false);
        int MaxSn();
        OperateResult AddOrUpdateResult(Warehouse obj);
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="Ids">主键</param>
        /// <param name="state">状态值</param>
        /// <param name="type">所有禁用或后台禁用</param>
        void SetState(string Ids, short state, short type);
    }
}
