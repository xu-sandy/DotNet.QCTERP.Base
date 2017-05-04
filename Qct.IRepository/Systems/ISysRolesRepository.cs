using System.Collections.Generic;
using System.Data;
using Qct.Objects.Entities;
using Qct.Infrastructure.Data;
using Qct.Objects.ValueObjects;

namespace Qct.IRepository
{
    public interface ISysRolesRepository: IEFRepository<SysRoles>
    {
        List<SysLimitsModel> GetAllLimitList(string roleids);
        DataTable GetAllList();
        DataTable GetList();
        SysRoles GetRole(int roleId);
        List<SysLimits> GetRoleLimitsByRoleId(string roleid);
        List<SysLimits> GetRoleLimitsByUId(string uid);
        List<SysRoles> GetRoleList(bool all = true);
        new OperateResult AddOrUpdate(SysRoles obj);
        void ChangeStatus(int id);
        void UpdateLimit(int roleid,string limitIds);
    }
}