using Qct.Infrastructure.Extensions;
using Qct.IRepository;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Qct.Repository
{
    public class SysMenusRepository : BaseEFRepository<SysMenus>, ISysMenusRepository
    {

        #region 菜单管理
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public List<SysMenus> GetList()
        {
            return GetReadOnlyEntities().OrderBy(o => o.SortOrder).ToList();
        }
        /// <summary>
        /// 根据角色或用户获取可配置菜单
        /// </summary>
        /// <param name="type">1-用户,2-角色</param>
        /// <param name="objIds">roleId,userId</param>
        /// <returns></returns>
        public List<SysMenus> GetMenusTreeList(int type, string objIds)
        {
            var sql = @"SELECT * FROM dbo.SysMenus m
				WHERE m.CompanyId=@CompanyId
				and (dbo.Comm_F_NumIsInGroup(@adminrole,@roleIds)=1 OR dbo.Comm_F_NumIsInGroup(@superrole,@roleIds)=1 or
				EXISTS(SELECT 1 FROM dbo.SplitString((SELECT MenuIds+',' FROM SysCustomMenus WHERE CompanyId=@CompanyId AND Type=@type AND dbo.Comm_F_NumIsInGroup(ObjId,@roleIds)=1 FOR XML PATH('')),',',1) WHERE Value=m.MenuId))";
            SqlParameter[] parms = {
					new SqlParameter("@roleIds", objIds),
					new SqlParameter("@type", type),
					new SqlParameter("@adminrole", SystemConfig.AdminRoleId),
					new SqlParameter("@superrole",  SystemConfig.SuperRoleId),
                    new SqlParameter("@CompanyId",CompanyId)
				};
            return _context.ExecuteQuery<SysMenus>(sql, parms).ToList();
        }
        
        
        /// <summary>
        /// 更新菜单排序号
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMenuOrderIndex(SysMenus model)
        {
            var obj= _context.Entry<SysMenus>(model);
            obj.State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            return true;
        }
        /// <summary>
        /// 查找菜单项对应父节点的所有子节点
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public List<SysMenus> FindParentChilds(int menuId)
        {
            return GetReadOnlyEntities().Where(o => o.PMenuId == menuId).ToList();
        }
        public int GetMenuMaxIndex(int pMenuId)
        {
            return FindParentChilds(pMenuId).Max(o=>o.SortOrder);
        }
        public SysMenus GetMenu(int menuId)
        {
            return GetReadOnlyEntities().FirstOrDefault(o => o.MenuId == menuId);
        }

        public int GetMaxMenuId()
        {
            return GetMaxValInt(o => o.MenuId);
        }

        #endregion 菜单管理

        #region 首页菜单
        /// <summary>
        /// 获得首页菜单列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="type">菜单类型 0-系统，1-门店</param>
        /// <returns></returns>
        public List<SysMenus> GetHomeMenusByUID(string uid,short type)
        {
            if (uid.IsNullOrEmpty())
                return GetReadOnlyEntities().Where(o => o.Type == type && o.Status).ToList();

            var sql = @"SELECT s.* FROM dbo.SysMenus s
                INNER JOIN(
                SELECT DISTINCT Value FROM dbo.SplitString((
                SELECT ',' + a.MenuIds FROM dbo.SysCustomMenus a
                INNER JOIN SysUserInfo b ON ',' + b.RoleIds + ',' LIKE '%,' + CAST(a.ObjId AS VARCHAR(10)) + ',%' AND a.Type = 2
                WHERE b.UID = @uid
                AND a.CompanyId = @companyId FOR XML PATH('')),',',1)) m ON m.Value = s.MenuId
                WHERE s.CompanyId = @companyId AND s.Status = 1 AND s.Type=@type ORDER BY s.SortOrder";
            SqlParameter[] parms = {
                    new SqlParameter("@companyId", CompanyId),
                    new SqlParameter("@uid", uid),
                    new SqlParameter("@type", type)
            };
            return _context.ExecuteQuery<SysMenus>(sql, parms).ToList();
        }
        #endregion 首页菜单
    }
}
