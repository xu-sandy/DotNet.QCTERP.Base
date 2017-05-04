using System.Collections.Generic;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;

namespace Qct.IServices
{
    public interface ISysMenuService
    {
        OperateResult ChangeStatus(int id);
        bool Delete(int id);
        bool Delete(params object[] ids);
        List<MenuModel> GetHomeMenusByUID(string uid, short type = 0);
        List<SysMenus> GetList();
        List<SysMenus> GetMenusTreeList(int type, string objIds);
        SysMenusModel GetModel(int id, int pobjid);
        OperateResult MoveMenuItem(int mode, int menuId);
        OperateResult SaveMenu(SysMenus model);
    }
}