using Qct.Objects.Entities;
using Qct.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qct.IRepository;
using Qct.Objects.ValueObjects;

namespace Qct.Repository
{
    public class SysRolesRepository : BaseEFRepository<SysRoles>, ISysRolesRepository
    {
        /// <summary>
        /// 获取角色列表数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetList()
        {
            string sql = @"select a.Id,a.RoleId,a.Title,a.LimitsIds,a.Memo,a.[Status]
	        ,(CASE WHEN (b.[ObjId] IS NOT null) THEN '已配置' ELSE '未配置' END) HasMenus
	        ,(CASE WHEN (a.LimitsIds<>'-1' AND LEN(a.LimitsIds)>0) THEN '已配置' ELSE '未配置' END) HasLimits
	        ,r1.UsersNum
	        from SysRoles a
	        LEFT JOIN (SELECT DISTINCT [ObjId] from dbo.SysCustomMenus) b ON b.[ObjId]=a.RoleId
	        LEFT JOIN (
		        SELECT RoleId,SUM(CONVERT(INT,num)) UsersNum 
		        FROM (
		        SELECT r.RoleId,r.Title,u.[UID],u.FullName,dbo.Comm_F_NumIsInGroup(r.roleId,u.RoleIds) num
		        FROM dbo.SysRoles r,dbo.SysUserInfo u WHERE r.CompanyId=u.CompanyId and u.CompanyId=@companyId) a
		        GROUP BY a.RoleId
	        ) r1 ON r1.RoleId=a.RoleId
	        where a.ShowView=1 AND a.companyId=@companyId";
            SqlParameter[] _params = { new SqlParameter("@companyId",CompanyId) };
            return GetDataTableBySql(sql, _params);
        }

        /// <summary>
        /// 获取所有角色列表数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllList()
        {
            string sql = @"select a.Id,a.RoleId,a.Title,a.LimitsIds,a.Memo,a.[Status]
	        ,(CASE WHEN (b.[ObjId] IS NOT null) THEN '已配置' ELSE '未配置' END) HasMenus
	        ,(CASE WHEN (a.LimitsIds<>'-1' AND LEN(a.LimitsIds)>0) THEN '已配置' ELSE '未配置' END) HasLimits
	        ,r1.UsersNum
	        from SysRoles a
	        LEFT JOIN (SELECT DISTINCT [ObjId] from dbo.SysCustomMenus) b ON b.[ObjId]=a.RoleId
	        LEFT JOIN (
		        SELECT RoleId,SUM(CONVERT(INT,num)) UsersNum 
		        FROM (
		        SELECT r.RoleId,r.Title,u.[UID],u.FullName,dbo.Comm_F_NumIsInGroup(r.roleId,u.RoleIds) num
		        FROM dbo.SysRoles r,dbo.SysUserInfo u WHERE r.CompanyId=u.CompanyId and u.CompanyId=@companyId) a
		        GROUP BY a.RoleId
	        ) r1 ON r1.RoleId=a.RoleId
	        where a.companyId=@companyId";
            SqlParameter[] _params = { new SqlParameter("@companyId", CompanyId) };
            return GetDataTableBySql(sql, _params);
        }

        public SysRoles GetRole(int roleId)
        {
            return GetEntities().FirstOrDefault(o=>o.RoleId==roleId);
        }

        /// <summary>
        /// 根据roleid获取sysrole
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public List<SysLimits> GetRoleLimitsByUId(string uid)
        {
            string sql = @"SELECT s.* FROM dbo.SysLimits s
                INNER JOIN(
                SELECT DISTINCT Value FROM dbo.SplitString((
                SELECT ','+a.LimitsIds FROM dbo.SysRoles a
                INNER JOIN SysUserInfo b ON ','+b.RoleIds+',' LIKE '%,'+CAST( a.RoleId AS VARCHAR(10))+',%'
                WHERE b.UID=@uid
                AND a.CompanyId=@companyId FOR XML PATH('')),',',1)) m ON m.Value=s.LimitId
                WHERE s.CompanyId=@companyId AND s.Status=1 ORDER BY s.SortOrder";
            SqlParameter[] parms = { new SqlParameter("@uid", uid),
                                   new SqlParameter("@companyId", CompanyId) };

            return _context.ExecuteQuery<SysLimits>(sql, parms).ToList();
        }
        /// <summary>
        /// 根据roleid获取sysrole
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public List<SysLimits> GetRoleLimitsByRoleId(string roleid)
        {
            string sql = @"SELECT s.* FROM dbo.SysLimits s
                INNER JOIN(
                SELECT DISTINCT Value FROM dbo.SplitString((
                SELECT a.LimitsIds FROM dbo.SysRoles a WHERE a.CompanyId=@companyId and a.RoleId=@RoleId
                ),',',1)) m ON m.Value=s.LimitId
                WHERE s.CompanyId=@companyId AND s.Status=1 ORDER BY s.SortOrder";

            SqlParameter[] parms = { new SqlParameter("@RoleId", roleid),
                                   new SqlParameter("@companyId", CompanyId) };

            return _context.ExecuteQuery<SysLimits>(sql, parms).ToList();
        }
        /// <summary>
        /// 获取所有的角色列表
        /// </summary>
        /// <returns></returns>
        public List<SysRoles> GetRoleList(bool all=true)
        {
            var query= GetReadOnlyEntities().Where(o => o.ShowView == 1);
            if (!all) query = query.Where(o => o.Status);
            return query.ToList();
        }
        
        /// <summary>
        /// 获得所有权限列表
        /// </summary>
        /// <returns></returns>
        public List<SysLimitsModel> GetAllLimitList(string roleids)
        {
            //var roleIds = new List<int>() { 4,9};
            //var rs = roleids.ToTypeArray<int>();
            //roleIds.AddRange(rs);
            //string sql = @"SELECT s.* FROM dbo.SysLimits s
            //    INNER JOIN(
            //    SELECT DISTINCT Value FROM dbo.SplitString((
            //    SELECT ','+a.LimitsIds FROM dbo.SysRoles a where a.RoleId IN(" + string.Join(",", roleIds) + @")
            //    AND a.CompanyId=@companyId FOR XML PATH('')),',',1)) m ON m.Value=s.LimitId
            //    WHERE s.CompanyId=@companyId AND s.Status<>0 ORDER BY s.SortOrder";
            //SqlParameter[] parms = { 
            //                       new SqlParameter("@companyId", CompanyId) };

            //return _context.ExecuteQuery<SysLimits>(sql, parms).ToList();
            SqlParameter[] _params ={
                new SqlParameter("@roleIds",roleids),
                new SqlParameter("@companyId",CompanyId)
            };
            return _context.ExecuteQuery<SysLimitsModel>("Sys_AllLimitList @roleIds,@companyId", _params).ToList();
        }

        public new OperateResult AddOrUpdate(SysRoles obj)
        {
            if (!obj.Title.IsNullOrEmpty() && IsExists(o => o.Id != obj.Id && o.Title == obj.Title))
                return OperateResult.Fail("角色名称不能重复!");
            if(obj.Id==0)
            {
                obj.RoleId = GetMaxValInt(o => o.RoleId);
                Create(obj);
            }
            else
            {
                var resource = Get(obj.Id);
                obj.ToCopyProperty(resource, true, "ShowView", "CompanyId");
            }
            SaveChanges();
            return OperateResult.Success();
        }
        
        public void ChangeStatus(int id)
        {
            var obj = Get(id);
            obj.Status = !obj.Status;
            SaveChanges();
        }

        public void UpdateLimit(int roleid, string limitIds)
        {
            var obj = GetRole(roleid);
            obj.LimitsIds = limitIds;
            SaveChanges();
        }
    }
}