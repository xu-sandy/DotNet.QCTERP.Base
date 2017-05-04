using System.Collections.Generic;
using Qct.Objects.Entities;
using Qct.Infrastructure.Data;

namespace Qct.IRepository
{
    public interface ISysMenusRepository: IEFRepository<SysMenus>
    {
        List<SysMenus> FindParentChilds(int menuId);
        List<SysMenus> GetHomeMenusByUID(string uid, short type);
        List<SysMenus> GetList();
        int GetMaxMenuId();
        SysMenus GetMenu(int menuId);
        int GetMenuMaxIndex(int pMenuId);
        /// <summary>
        /// 根据角色或用户获取可配置菜单
        /// </summary>
        /// <param name="type">1-用户,2-角色</param>
        /// <param name="objIds">roleId,userId</param>
        /// <returns></returns>
        List<SysMenus> GetMenusTreeList(int type, string objIds);
        bool UpdateMenuOrderIndex(SysMenus model);
    }
}