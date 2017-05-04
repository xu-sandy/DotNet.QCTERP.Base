using Qct.Objects.ValueObjects;
using Qct.Repository;
using Qct.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Qct.Objects.Entities;
using Qct.IServices;
using Qct.IRepository;

namespace Qct.Services
{
    /// <summary>
    /// 菜单管理业务逻辑
    /// </summary>
    public class SysMenuService : ISysMenuService
    {
        ISysMenusRepository _sysMenusRepository { get; set; }
        public SysMenuService(ISysMenusRepository sysMenusRepository)
        {
            _sysMenusRepository = sysMenusRepository;
        }
        #region 首页菜单
        /// <summary>
        /// 获得允许用户访问的菜单列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<MenuModel> GetHomeMenusByUID(string uid,short type=0)
        {
            var objs = _sysMenusRepository.GetHomeMenusByUID(uid,type).Select(o=>new MenuModel() {
                Id=o.MenuId.ToString(),
                Level=0,
                Name=o.Title,
                ParentId=o.PMenuId.ToString(),
                Url=o.URL
            });
            List<MenuModel> models = new List<MenuModel>();


            objs.Where(t => t.ParentId == "0").Each(t =>
            {
                t.Level = 0;
                models.Add(GetMenusChildsTreeData(t, objs));
            });
            

            return models;
        }
        /// <summary>
        /// 获得父级菜单的子节点
        /// </summary>
        /// <param name="model"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private MenuModel GetMenusChildsTreeData(MenuModel model, IEnumerable<MenuModel> source)
        {
            var childs = source.Where(s => s.ParentId == model.Id);
            if (childs.Count() > 0)
            {
                model.Children = new List<MenuModel>();
                childs.Each(t =>
                {
                    t.Level = model.Level + 1;
                    model.Children.Add(GetMenusChildsTreeData(t, source));
                });
            }
            return model;
        }
        #endregion

        #region 菜单管理
        /// <summary>
        /// 查找全部数据
        /// </summary>
        /// <returns></returns>
        public List<SysMenus> GetList()
        {
            return _sysMenusRepository.GetList();
        }
        /// <summary>
        /// 根据角色或用户获取可配置菜单
        /// </summary>
        /// <param name="type">1-用户,2-角色</param>
        /// <param name="objIds">roleId,userId</param>
        /// <returns></returns>
        public List<SysMenus> GetMenusTreeList(int type, string objIds)
        {
            return _sysMenusRepository.GetMenusTreeList(type, objIds);
        }
        /// <summary>
        /// 移动菜单，更改菜单排序
        /// </summary>
        /// <param name="mode">1：上移，2：下移</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns></returns>
        public OperateResult MoveMenuItem(int mode, int menuId)
        {
            var result = OperateResult.Fail("菜单项排序变更失败");
            try
            {
                var childrens = _sysMenusRepository.FindParentChilds(menuId);
                var currentMenu = childrens.FirstOrDefault(o => o.MenuId == menuId);
                if (currentMenu != null)
                {
                    switch (mode)
                    {
                        case 1:
                            var minSortOrder = childrens.Min(o => o.SortOrder);
                            if (currentMenu.SortOrder > minSortOrder)
                            {
                                SysMenus previousMenu = null;
                                int i = 1;
                                while (previousMenu == null && (currentMenu.SortOrder - i) >= minSortOrder)
                                {
                                    previousMenu = childrens.FirstOrDefault(o => o.SortOrder == (currentMenu.SortOrder - i));
                                    i++;
                                }
                                var sortOrder = currentMenu.SortOrder;
                                currentMenu.SortOrder = previousMenu.SortOrder;
                                previousMenu.SortOrder = sortOrder;

                                _sysMenusRepository.UpdateMenuOrderIndex(previousMenu);
                                _sysMenusRepository.UpdateMenuOrderIndex(currentMenu);
                            }
                            break;
                        case 2:
                            var maxSortOrder = childrens.Max(o => o.SortOrder);
                            if (currentMenu.SortOrder < maxSortOrder)
                            {
                                SysMenus previousMenu = null;
                                int i = 1;
                                while (previousMenu == null && (currentMenu.SortOrder + i) <= maxSortOrder)
                                {
                                    previousMenu = childrens.FirstOrDefault(o => o.SortOrder == (currentMenu.SortOrder + i));
                                    i++;
                                }
                                var sortOrder = currentMenu.SortOrder;
                                currentMenu.SortOrder = previousMenu.SortOrder;
                                previousMenu.SortOrder = sortOrder;
                                _sysMenusRepository.UpdateMenuOrderIndex(previousMenu);
                                _sysMenusRepository.UpdateMenuOrderIndex(currentMenu);
                            }
                            break;
                    }
                }
                result = OperateResult.Success("数据保存成功");
            }
            catch (Exception e)
            {
                result = OperateResult.Fail("菜单项排序变更失败" + e.Message);
            }
            return result;
        }

        /// <summary>
        /// 更改菜单状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OperateResult ChangeStatus(int id)
        {
            var result = OperateResult.Fail("状态变更失败");
            try
            {
                var model = _sysMenusRepository.Get(id);
                model.Status = !model.Status;
                _sysMenusRepository.SaveChanges();
                result = OperateResult.Success("数据保存成功");
            }
            catch (Exception e)
            {
                result = OperateResult.Fail("状态变更失败" + e.Message);
            }
            return result;
        }
        /// <summary>
        /// 根据Id获得菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pobjid"></param>
        /// <returns></returns>
        public SysMenusModel GetModel(int id, int pobjid)
        {
            var objExt = new SysMenusModel();
            var obj = _sysMenusRepository.Get(id);
            obj.ToCopyProperty(objExt);
            if (pobjid != 0)
            {
                var pobj = _sysMenusRepository.GetMenu(pobjid);
                if (pobj != null)
                {
                    objExt.PMenuId = pobj.MenuId;
                    objExt.PTitle = pobj.Title;
                    objExt.Type = pobj.Type;
                }
            }
            return objExt;
        }
        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OperateResult SaveMenu(SysMenus model)
        {
            var result = OperateResult.Fail("数据保存失败!");
            try
            {//todo: Set Depth
                if (model.Id>0)
                {
                    var obj = _sysMenusRepository.Get(model.Id);
                    model.ToCopyProperty(obj);
                    _sysMenusRepository.SaveChanges();
                    result = OperateResult.Success("数据保存成功");
                }
                else
                {
                    model.MenuId = _sysMenusRepository.GetMaxMenuId();
                    model.SortOrder = _sysMenusRepository.GetMenuMaxIndex(model.PMenuId) + 1;
                    _sysMenusRepository.CreateWithSaveChanges(model);
                    result = OperateResult.Success("数据保存成功");
                }
            }
            catch (Exception ex)
            {
                result = OperateResult.Fail("数据保存失败!" + ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            _sysMenusRepository.DeleteWithSaveChanges(id);
            return true;
        }

        public bool Delete(params object[] ids)
        {
            _sysMenusRepository.DeleteWithSaveChanges(ids);
            return true;
        }
        #endregion
    }
}
