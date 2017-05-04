using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.Extensions;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Qct.IRepository
{
    public interface ISysUserRespository: IEFRepository<Qct.Objects.Entities.SysUserInfo>
    {
        SysUserInfo Login(string account, string password);
        List<SysUserInfo> GetList(bool all, string selectUid = "");
        PageInformaction<System.Data.DataTable> FindPageList(NameValueCollection nvl);
        OperateResult AddOrUpdate(SysUserInfo entity);
        OperateResult Delete(int id,string uid);
        string MaxCode();
        /// <summary>
        /// 获取所有门店角色设置数据
        /// </summary>
        /// <returns></returns>
        System.Data.DataTable GetUserStoreRoles();
        /// <summary>
        /// 保存门店角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        OperateResult SaveStoreUserInfo(SysStoreUserInfo model);

        SysUserInfo GetByUid(string uid);
    }
}
