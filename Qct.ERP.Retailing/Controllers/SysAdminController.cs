using System.Web.Mvc;
using System.Linq;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using Qct.Infrastructure.Json;
using Qct.Infrastructure.Web.Extensions;
using Qct.IRepository;
using System.Collections.Generic;
using Qct.Infrastructure.Extensions;
using Autofac.Integration.Mvc;
using Qct.Infrastructure.Helpers;
using System.IO;
using Qct.ERP.ApplicationService;
using Qct.IServices;

namespace Qct.ERP.Retailing.Controllers
{
    public class SysAdminController : Controller
    {
        #region 实例化对象
        readonly ISysDepartmentRepository _departmentRepository = null;
        readonly ISysDictionaryRepository _sysDictionaryRepository = null;
        readonly ISysUserRespository _sysUserRespository = null;
        readonly ISysRolesRepository _sysRoleRespository = null;
        readonly IWarehouseRepository _warehouseRepository = null;
        readonly ISysLogRepository _syslogRepository = null;
        readonly IApiLibraryRepository _apiLibraryRepository = null;
        readonly SaleImportService _saleImportService = null;
        readonly IImportSetService _importSetService = null;
        public SysAdminController(ISysDictionaryRepository sysDictionaryRepository, ISysDepartmentRepository departmentRepository, ISysUserRespository sysUserRespository,
            ISysRolesRepository sysRoleRespository, IWarehouseRepository warehouseRepository, ISysLogRepository syslogRepository, 
            IApiLibraryRepository apiLibraryRepository, SaleImportService saleImportService, IImportSetService importSetService)
        {
            _departmentRepository = departmentRepository;
            _sysDictionaryRepository = sysDictionaryRepository;
            _sysUserRespository = sysUserRespository;
            _sysRoleRespository = sysRoleRespository;
            _warehouseRepository = warehouseRepository;
            _syslogRepository = syslogRepository;
            _apiLibraryRepository = apiLibraryRepository;
            _saleImportService = saleImportService;
            _importSetService = importSetService;
        }
        #endregion

        #region 数据字典

        public ActionResult Dic()
        {
            return View();
        }
        public ActionResult GetDict(int page = 1, int rows = 20, string key = "")
        {
            var result = _sysDictionaryRepository.GetList(page, rows, key);
            return this.ToDataGrid(result, result.Count());
        }
        public ActionResult ShowChildDict(int id, int psn)
        {
            ViewData["psn"] = psn;
            return View();
        }
        public ActionResult GetDictItems(int psn)
        {
            var entity = _sysDictionaryRepository.GetItemsByDicpsn(psn);
            return this.ToDataGrid(entity, entity.Count());
        }
        public ActionResult AddDict(int id = -1, int psn = 0)
        {
            var model = _sysDictionaryRepository.GetExtModel(id, psn);
            return View(model);
        }
        public ActionResult SwitchStatus(int sn)
        {
            var result = _sysDictionaryRepository.ChangeStatus(sn);
            return Content(result.ToJson());
        }
        [HttpPost]
        public ActionResult AddDict(SysDataDictionary model)
        {
            var re = new OperateResult() { Successed = true };
            var result = _sysDictionaryRepository.SaveModel(model);
            return Content(result.ToJson());
        }
        public ActionResult AddDictTitle(string title, int psn)
        {
            var re = new OperateResult() { Successed = true };
            var obj = new SysDataDictionary();
            var source = _sysDictionaryRepository.GetItemByTitle(title);
            if (source != null)
                re.Message = source.DicSN.ToString();
            else
            {
                obj.Title = title;
                obj.Status = true;
                obj.DicSN = _sysDictionaryRepository.GetMaxDicSn;
                obj.DicPSN = psn;
                re = _sysDictionaryRepository.SaveModel(obj);
                if (re.Successed) re.Message = obj.DicSN.ToString();
            }
            return Content(re.ToJson());
        }
        public ActionResult MoveDataItem(int mode, int sn)
        {
            var op = _sysDictionaryRepository.MoveItem(mode, sn);
            return this.ToJsonResult(op);
        }
        #endregion

        #region 组织机构
        /// <summary>
        /// 组织机构-页面加载
        /// </summary>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.组织机构_查看机构或部门)]
        public ActionResult Org()
        {
            return View();
        }

        /// <summary>
        /// 组织机构-页面数据加载
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.组织机构_查看机构或部门)]
        public ActionResult GetOrg()
        {
            var datas = _departmentRepository.GetExtList();
            //构造树数据
            List<SysDepartmentsModel> models = new List<SysDepartmentsModel>();
            if (datas != null)
            {
                datas.Where(t => t.PDepId == 0).Each(t =>
                {
                    models.Add(GetOrgChildsEasyuiGridData(t, datas));
                });
            }
            return this.ToDataGrid(models, 2);
        }
        /// <summary>
        /// 组织机构-新增或编辑组织机构表单-页面加载
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.组织机构_查看机构或部门)]
        public ActionResult OrgSave(int id = 0, int pdepid = 0)
        {
            var model = _departmentRepository.GetModel(id, pdepid);
            return View(model);
        }
        /// <summary>
        /// 组织机构-新增或编辑组织机构表单-保存方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.组织机构_创建机构或部门)]
        public ActionResult OrgSave(SysDepartments model)
        {
            var result = _departmentRepository.SaveDep(model);
            return this.ToJsonOperateResult(result);
        }
        /// <summary>
        /// 组织机构-修改组织机构状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.组织机构_关闭或显示机构部门)]
        public ActionResult ChangeOrgStatusById(int id)
        {
            _departmentRepository.ChangeStatus(id);
            return this.ToJsonOperateResult();
        }
        /// <summary>
        /// 组织机构-移除组织机构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.组织机构_删除机构或部门)]
        public ActionResult RemoveOrg(int id)
        {
            _departmentRepository.DeleteWithSaveChanges(id);
            return this.ToJsonOperateResult();
        }
        /// <summary>
        /// 组织机构-新建组织机构表单-构造组织机构下拉树数据对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost]
        [HttpGet]
        //[SysPermissionValidate(Code = Sys.SysConstLimits.组织机构_查看机构或部门)]
        public ActionResult GetOrgTreeList(string id)
        {
            var datas = _departmentRepository.GetExtList();
            //构造树数据
            List<EasyuiTree> models = new List<EasyuiTree>();
            datas.Where(t => t.PDepId == 0).Each(t =>
            {
                models.Add(GetOrgChildsEasyuiTreeData(t, datas));
            });
            return this.ToJsonResult(models);
        }
        #region private
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private SysDepartmentsModel GetOrgChildsEasyuiGridData(SysDepartmentsModel model, List<SysDepartmentsModel> source)
        {
            var childs = source.Where(s => s.PDepId == model.DepId);
            if (childs.Count() > 0)
            {
                model.Childs = new List<SysDepartmentsModel>();
                childs.Each(t =>
                {
                    model.Childs.Add(GetOrgChildsEasyuiGridData(t, source));
                });
            }
            return model;
        }
        /// <summary>
        /// 构造菜单下拉树子集数据
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private EasyuiTree GetOrgChildsEasyuiTreeData(SysDepartmentsModel data, List<SysDepartmentsModel> source)
        {
            var model = new EasyuiTree { id = data.DepId.ToString(), text = data.Title };
            var childs = source.Where(s => s.PDepId == data.DepId);
            if (childs.Count() > 0)
            {
                model.children = new List<EasyuiTree>();
                childs.Each(t =>
                {
                    model.children.Add(GetOrgChildsEasyuiTreeData(t, source));
                });
            }
            return model;
        }
        #endregion
        #endregion 组织机构

        #region 用户管理

        /// <summary>
        /// 用户管理-页面加载
        /// </summary>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.用户管理_查看用户)]
        public ActionResult Users()
        {
            return View();
        }
        /// <summary>
        /// 用户管理-页面数据加载
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUsers(int page = 1, int rows = 30)
        {
            var pageobj = _sysUserRespository.FindPageList(Request.Params);
            return this.ToDataGrid(pageobj.Datas.FirstOrDefault(), pageobj.CollectinSize);
        }
        /// <summary>
        /// 用户管理-新增或编辑用户表单-页面加载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.用户管理_查看用户)]
        public ActionResult UserSave(int id = 0)
        {
            SysUserInfo model = null;
            if (id > 0) model = _sysUserRespository.Get(id);
            if (model == null)
            {
                model = new SysUserInfo();
            }
            ViewBag.sysUserState = this.ToSelectTitle(new List<SelectListItem>() { new SelectListItem() { Text="正常",Value="1"},
                new SelectListItem() { Text = "锁定", Value = "2" },new SelectListItem() { Text = "注销", Value = "3" } }, emptyTitle: "请选择");
            var roleService = AutofacDependencyResolver.Current.GetService<ISysRolesRepository>();
            ViewBag.roles = this.ToSelectTitle(roleService.GetRoleList().Select(o => new SelectListItem() { Value = o.RoleId.ToString(), Text = o.Title }));
            return View(model);
        }
        /// <summary>
        /// 用户管理-新增或编辑用户表单-保存用户方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.用户管理_创建用户)]
        public ActionResult UserSave(SysUserInfo model)
        {
            model.RoleIds = Request["RoleIds"];
            model.CreateUID = Services.SysUserService.CurrentUser.UserID;
            var result = _sysUserRespository.AddOrUpdate(model);
            return this.ToJsonOperateResult(result);
        }
        /// <summary>
        /// 用户管理-根据用户Id移除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.用户管理_物理删除用户)]
        public ActionResult DeleteUser(int id, string uid)
        {
            var re = _sysUserRespository.Delete(id, Services.SysUserService.CurrentUser.UserID);
            return Content(re.ToJson());
        }

        #region 门店角色设置
        /// <summary>
        /// 用户管理-门店角色设置
        /// </summary>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.用户管理_门店角色设置)]
        public ActionResult UserSettingStoreRole(string uid)
        {
            var obj = AutofacDependencyResolver.Current.GetService<ISysStoreUserInfoRepository>().GetByUid(uid);
            if (obj == null)
            {
                obj = new SysStoreUserInfo() { UID = uid };
            }
            return View(obj);
        }
        /// <summary>
        /// 用户管理-门店角色设置数据加载
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.用户管理_门店角色设置)]
        public ActionResult GetUserSettingStoreRole()
        {
            var objs = _sysUserRespository.GetUserStoreRoles();
            return this.ToDataGrid(objs, objs.Rows.Count);
        }
        /// <summary>
        /// 用户管理-门店角色设置数据保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.用户管理_门店角色设置)]
        public ActionResult UserSettingStoreRole(SysStoreUserInfo model)
        {
            var result = _sysUserRespository.SaveStoreUserInfo(model);
            return Content(result.ToJson());
        }
        #endregion 门店角色设置

        #endregion 用户管理

        #region 角色管理

        #region 角色管理页
        /// <summary>
        /// 角色管理-页面加载
        /// </summary>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.角色管理_查看角色)]
        public ActionResult Roles()
        {
            return View();
        }
        /// <summary>
        /// 角色管理-页面数据加载
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.角色管理_查看角色)]
        public ActionResult GetRoles()
        {
            var entities = Services.SysUserService.CurrentUser.ErpUserRoles == "9" ? _sysRoleRespository.GetAllList() : _sysRoleRespository.GetList();
            return this.ToDataGrid(entities, entities.Rows.Count);
        }

        /// <summary>
        /// 角色管理-新增或编辑角色表单-页面加载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.角色管理_查看角色)]
        public ActionResult RoleSave(int id = 0)
        {
            var model = _sysRoleRespository.Get(id) ?? new SysRoles();
            return View(model);
        }
        /// <summary>
        /// 角色管理-新增或编辑角色表单-角色保存
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.角色管理_创建角色)]
        public ActionResult RoleSave(SysRoles model)
        {
            model.LimitsIds = model.LimitsIds ?? "";
            model.ShowView = 1;
            var result = _sysRoleRespository.AddOrUpdate(model);
            return this.ToJsonOperateResult(result);
        }
        /// <summary>
        /// 角色管理-更改角色状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.角色管理_状态设定)]
        public ActionResult CloseRole(int id)
        {
            _sysRoleRespository.ChangeStatus(id);
            return this.ToJsonOperateResult();
        }
        /// <summary>
        /// 角色管理-移除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.角色管理_物理删除角色)]
        public ActionResult RemoveRole(int id)
        {
            _sysRoleRespository.DeleteWithSaveChanges(id);
            return this.ToJsonOperateResult();
        }
        #endregion 角色管理

        #region 角色菜单设置
        /// <summary>
        /// 角色管理-角色菜单设置-页面数据加载
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.角色管理_菜单设置)]
        public ActionResult RoleSettingMenus(int roleId)
        {
            var model = AutofacDependencyResolver.Current.GetService<ISysCustomMenusRepository>().GetbyObjid(roleId) ?? new SysCustomMenus() { ObjId = roleId };
            return View(model);
        }
        /// <summary>
        /// 角色管理-角色菜单设置-页面加载
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.角色管理_菜单设置)]
        public ActionResult GetRoleSettingMenus(string roleId)
        {
            return View();
        }
        /// <summary>
        /// 角色管理-角色菜单设置-角色菜单保存
        /// </summary>
        /// <param name="syscustomMenus"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.角色管理_菜单设置)]
        public ActionResult SaveRoleSettingMenus(SysCustomMenus syscustomMenus)
        {
            syscustomMenus.Type = 2;
            AutofacDependencyResolver.Current.GetService<ISysCustomMenusRepository>().AddOrUpdate(syscustomMenus);
            return this.ToJsonOperateResult();
        }
        /// <summary>
        /// 角色管理-角色菜单设置-菜单下拉树数据源
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.角色管理_菜单设置)]
        public ActionResult GetMenusTreeLists(string roleId, short type = 0)
        {
            var roleStr = Services.SysUserService.CurrentUser.ErpUserRoles;
            var src = AutofacDependencyResolver.Current.GetService<ISysMenusRepository>();
            var datas = src.GetMenusTreeList(2, roleStr);
            var roleids = roleStr.ToTypeArray<int>();
            if (!(roleids.Contains(SystemConfig.AdminRoleId) || roleids.Contains(SystemConfig.SuperRoleId)))
            {
                var seldatas = src.GetMenusTreeList(2, roleId);
                datas = seldatas.Where(o => datas.Select(i => i.MenuId).Contains(o.MenuId)).ToList();
            }
            datas = datas.Where(o => o.Type == type).ToList();
            //构造树数据
            List<EasyuiTree> models = new List<EasyuiTree>();
            datas.Where(t => t.PMenuId == 0).Each(t =>
            {
                models.Add(GetMenusChildsEasyuiTreeData(t, datas));
            });
            return this.ToJsonResult(models);
        }
        private EasyuiTree GetMenusChildsEasyuiTreeData(SysMenus menu, List<SysMenus> source)
        {
            var model = new EasyuiTree { id = menu.MenuId.ToString(), text = menu.Title };
            var childs = source.Where(s => s.PMenuId == menu.MenuId && s.Status == true);
            if (childs.Count() > 0)
            {
                model.children = new List<EasyuiTree>();
                childs.Each(t =>
                {
                    model.children.Add(GetMenusChildsEasyuiTreeData(t, source));
                });
            }
            return model;
        }
        #endregion 角色菜单设置

        #region 角色权限设置
        /// <summary>
        /// 角色管理-角色权限设置-权限页面加载
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.角色管理_权限设置)]
        public ActionResult RoleSettingLimits(int id = 0, string roleid = "0")
        {
            var model = _sysRoleRespository.Get(id) ?? new SysRoles();
            return View(model);
        }
        /// <summary>
        /// 角色管理-角色权限设置-权限页面数据加载
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.角色管理_权限设置)]
        public ActionResult GetRoleSettingLimits()
        {
            var datas = _sysRoleRespository.GetAllLimitList(Services.SysUserService.CurrentUser.ErpUserRoles);
            //构造树数据
            List<SysLimitsModel> models = new List<SysLimitsModel>();
            datas.Where(t => t.PLimitId == 0).Each(t =>
            {
                models.Add(GetChildsRoleAllLimits(t, datas));
            });
            return this.ToDataGrid(models, 2);
        }
        /// <summary>
        /// 角色管理-角色权限设置-保存角色权限数据
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="limitIds"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.角色管理_权限设置)]
        public ActionResult SaveRoleSettingLimits(int roleid, string limitIds)
        {
            _sysRoleRespository.UpdateLimit(roleid, limitIds);
            return this.ToJsonOperateResult();
        }
        /// <summary>
        /// 角色管理-角色权限设置-获取权限数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(Code = SysConstLimits.角色管理_权限设置)]
        private SysLimitsModel GetChildsRoleAllLimits(SysLimitsModel model, List<SysLimitsModel> source)
        {
            var childs = source.Where(s => s.PLimitId == model.LimitId);
            if (childs.Count() > 0)
            {
                model.Childs = new List<SysLimitsModel>();
                childs.Each(t =>
                {
                    model.Childs.Add(GetChildsRoleAllLimits(t, source));
                });
            }
            return model;
        }
        #endregion 角色权限设置

        #endregion 角色管理

        #region 门店维护
        //仓库维护
        public ActionResult Store()
        {
            ViewBag.states = this.ToSelectTitle(new List<SelectListItem>() {
                new SelectListItem() { Value = "1", Text = "经营" }, new SelectListItem() { Value = "0", Text = "停业" } }, emptyTitle: "请选择");
            return View();
        }
        [HttpPost]
        public ActionResult FindStorePageList()
        {
            int count = 0;
            var list = _warehouseRepository.FindPageList(Request.Params);
            return this.ToDataGrid(list, count);
        }

        /// <summary>
        /// 新增门店
        /// </summary>
        /// <returns></returns>
        public ActionResult AddStore(int? id)
        {
            
            ViewBag.categorys = AutofacDependencyResolver.Current.GetService<IProductCategoryRepository>().
                GetRootCategorys().Select(o => new SelectListItem(){
                    Text = o.Title,
                    Value = o.CategorySN.ToString()
                }).ToList();
            Warehouse model = null;
            if (id != null)
            {
                model = _warehouseRepository.Get(id);
            }
            return View(model??new Warehouse());
        }
        [HttpPost]
        public ActionResult AddStore(Warehouse obj)
        {
            obj.CategorySN = Request["CategorySN"];
            obj.CreateUID = Services.SysUserService.CurrentUser.UserID;
            var op= _warehouseRepository.AddOrUpdateResult(obj);
            return this.ToJsonOperateResult(op);
        }
        [HttpPost]
        public ActionResult SetStortState(string Ids, short state, short type)
        {
            _warehouseRepository.SetState(Ids, state, type);
            return this.ToJsonOperateResult();
        }
        #endregion

        #region 日志管理
        /// <summary>
        /// 日志管理-页面加载
        /// </summary>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.日志管理_查看日志)]
        public ActionResult Log()
        {
            return View();
        }
        /// <summary>
        /// 日志管理-页面数据加载
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [SysPermissionValidate(Code = SysConstLimits.日志管理_查看日志)]
        public ActionResult GetLogs()
        {
            var page = _syslogRepository.FindPageList(Request.Params);
            var result = (System.Data.DataTable)page.Datas.ElementAt(0);
            if (result.Rows.Count > 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    var colSummary = result.Rows[i]["Summary"];
                    var summary = colSummary == null ? "" : System.Web.HttpUtility.HtmlEncode(colSummary.ToString());
                    if (summary.Length > 180)
                        result.Rows[i]["Summary"] = "<div title='" + summary + "'>" + summary.Substring(0, 180) + "...</div>";
                }
            }
            return this.ToDataGrid(result, page.CollectinSize);
        }
        /// <summary>
        /// 日志管理-弹窗查看日志详情
        /// </summary>
        /// <param name="id">日志ID</param>
        /// <returns></returns>
        public ActionResult LogView(int id)
        {
            var model = _syslogRepository.Get(id);
            var user = _sysUserRespository.GetByUid(model.UId);
            ViewBag.UserLoginName = user == null ? model.UId : user.LoginName;
            return View(model);
        }
        /// <summary>
        /// 日志管理-删除所选日志
        /// </summary>
        /// <param name="Ids">日志ID数组</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteLogs(int[] Ids)
        {
            _syslogRepository.DeleteIds(Ids);
            return this.ToJsonOperateResult();
        }
        /// <summary>
        /// 日志管理-删除全部日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAll()
        {
            _syslogRepository.DeleteAll();
            return this.ToJsonOperateResult();
        }
        #endregion

        #region 基本配置
        //[SysPermissionValidate(Code = Sys.SysConstLimits.系统管理_基本配置)]
        public ActionResult WebSetting()
        {
            var model = AutofacDependencyResolver.Current.GetService<ISysWebSettingRepository>().GetWebSetting();
            if (model == null)
            {
                //OpResult op = null;
                //var comp = Authorize.GetCompanyByConnect(ref op);
                //var name = comp == null ? "" : comp.Title;
                var name = "";
                model = new SysWebSetting() { LogoDispWay = 1, SysName = name };
            }
            return View(model);
        }

        /// <summary>
        /// 系统管理-更新基本配置信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        //[SysPermissionValidate(Code = Sys.SysConstLimits.系统管理_基本配置)] 
        public ActionResult WebSetting(SysWebSetting model)
        {
            var relativePath = "";
            var path = FileHelper.SaveLogoPath(ref relativePath, Services.SysUserService.CurrentUser.CompanyId.ToString());
            if (Request.Files["LoginLogo"] != null && Request.Files["LoginLogo"].ContentLength != 0)
            {
                var topLogo = Request.Files["LoginLogo"];
                var topLogoName = "logo_login.png";
                string fullname = path + topLogoName;
                topLogo.SaveAs(fullname);
                model.LoginLogo = topLogoName;
            }
            if (Request.Files["TopLogo"] != null && Request.Files["TopLogo"].ContentLength != 0)
            {
                var topLogo = Request.Files["TopLogo"];
                var topLogoName = "logo_top.png";
                string fullname = path + topLogoName;
                topLogo.SaveAs(fullname);
                model.TopLogo = topLogoName;
            }
            if (Request.Files["hidBottomLogo"] != null && Request.Files["hidBottomLogo"].ContentLength != 0)
            {
                var bottomLogo = Request.Files["hidBottomLogo"];
                var bottomLogoName = "logo_bottom.png";
                string fullname = path + bottomLogoName;
                bottomLogo.SaveAs(fullname);
                model.BottomLogo = bottomLogoName;
            }
            var result = new OperateResult();
            if (Request.Files["hidAppIcon640"] != null && Request.Files["hidAppIcon640"].ContentLength != 0)
            {
                var appIcon = Request.Files["hidAppIcon640"];
                var appIconName = "logo_app_icon_640.png";
                string fullname = path + appIconName;
                appIcon.SaveAs(fullname);
                model.AppIcon640 = appIconName;
            }
            if (Request.Files["hidAppIcon960"] != null && Request.Files["hidAppIcon960"].ContentLength != 0)
            {
                var appIcon = Request.Files["hidAppIcon960"];
                var appIconName = "logo_app_icon_960.png";
                string fullname = path + appIconName;
                appIcon.SaveAs(fullname);
                model.AppIcon960 = appIconName;
            }
            if (Request.Files["hidAppIndexIcon640"] != null && Request.Files["hidAppIndexIcon640"].ContentLength != 0)
            {
                var appIcon = Request.Files["hidAppIndexIcon640"];
                var appIconName = "logo_app_index_640.png";
                string fullname = path + appIconName;
                appIcon.SaveAs(fullname);
                model.AppIndexIcon640 = appIconName;
            }
            if (Request.Files["hidAppIndexIcon960"] != null && Request.Files["hidAppIndexIcon960"].ContentLength != 0)
            {
                var appIcon = Request.Files["hidAppIndexIcon960"];
                var appIconName = "logo_app_index_960.png";
                string fullname = path + appIconName;
                appIcon.SaveAs(fullname);
                model.AppIndexIcon960 = appIconName;
            }
            if (Request.Files["hidAppCustomer640"] != null && Request.Files["hidAppCustomer640"].ContentLength != 0)
            {
                var appCustomer = Request.Files["hidAppCustomer640"];
                var appCustomerName = "app_index_customer_640.png";
                string fullname = path + appCustomerName;
                appCustomer.SaveAs(fullname);
                model.AppCustomer640 = appCustomerName;
            }
            if (Request.Files["hidAppCustomer960"] != null && Request.Files["hidAppCustomer960"].ContentLength != 0)
            {
                var appCustomer = Request.Files["hidAppCustomer960"];
                var appCustomerName = "app_index_customer_960.png";
                string fullname = path + appCustomerName;
                appCustomer.SaveAs(fullname);
                model.AppCustomer960 = appCustomerName;
            }
            if (Request.Files["hidAppIndexbg640"] != null && Request.Files["hidAppIndexbg640"].ContentLength != 0)
            {
                var appCustomer = Request.Files["hidAppIndexbg640"];
                var appCustomerName = "app_index_bg_640.png";
                string fullname = path + appCustomerName;
                appCustomer.SaveAs(fullname);
                model.AppIndexbg640 = appCustomerName;
            }
            if (Request.Files["hidAppIndexbg960"] != null && Request.Files["hidAppIndexbg960"].ContentLength != 0)
            {
                var appCustomer = Request.Files["hidAppIndexbg960"];
                var appCustomerName = "app_index_bg_960.png";
                string fullname = path + appCustomerName;
                appCustomer.SaveAs(fullname);
                model.AppIndexbg960 = appCustomerName;
            }
            if (Request.Files["hidSysIcon"] != null && Request.Files["hidSysIcon"].ContentLength != 0)
            {
                var rootPath = FileHelper.GetRoot;
                var sysIconPath = System.IO.Path.Combine(rootPath);

                var sysIcon = Request.Files["hidSysIcon"];
                var sysIconName = "favicon.ico";
                string fullname = sysIconPath + sysIconName;
                sysIcon.SaveAs(fullname);
                model.SysIcon = sysIconName;
            }
            if (result.Message.IsNullOrEmpty())
                AutofacDependencyResolver.Current.GetService<ISysWebSettingRepository>().AddOrUpdate(model);
            return this.ToJsonOperateResult();
        }


        #endregion

        #region 支付API
        public ActionResult ApiLibrarys()
        {
            var dictList= AutofacDependencyResolver.Current.GetService<ISysDictionaryRepository>().GetItemsByDicpsn(10);
            ViewBag.apiTypes = this.ToSelectTitle(dictList.Select(o => new SelectListItem() { Text = o.Title, Value = o.DicSN.ToString() }), emptyTitle: "全部");
            ViewBag.states = this.EnumToSelectList(typeof(EnableState), emptyTitle: "全部");
            return View();
        }
        public ActionResult ApiLibraryPageList(short? apiType, string keyword, short? state)
        {
            var page = _apiLibraryRepository.FindPageList(Request.Params);
            return this.ToDataGrid(page.Datas, page.CollectinSize);
        }
        public ActionResult ApiLibrarySave(int? id)
        {
            var dictList = AutofacDependencyResolver.Current.GetService<ISysDictionaryRepository>().GetItemsByDicpsn(10);
            ViewBag.apiTypes = this.ToSelectTitle(dictList.Select(o => new SelectListItem() { Text = o.Title, Value = o.DicSN.ToString() }), emptyTitle: "请选择");
            var obj = new ApiLibrary() { ReqMode = 1, State = 1 };
            if (id.HasValue)
            {
                obj = _apiLibraryRepository.Get(id);
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult ApiLibrarySave(ApiLibrary obj)
        {
            var relativePath = "";
            var path = FileHelper.SaveLogoPath(ref relativePath, "PayIcon");
            if (Request.Files["ApiIcon2"] != null && Request.Files["ApiIcon2"].ContentLength != 0)
            {
                var file = Request.Files["ApiIcon2"];
                var fileName = Path.GetFileName(file.FileName);
                string fullname = path + fileName;
                file.SaveAs(fullname);
                obj.ApiIcon = relativePath + fileName;
            }
            if (Request.Files["ApiCloseIcon2"] != null && Request.Files["ApiCloseIcon2"].ContentLength != 0)
            {
                var file = Request.Files["ApiCloseIcon2"];
                var fileName = Path.GetFileName(file.FileName);
                string fullname = path + fileName;
                file.SaveAs(fullname);
                obj.ApiCloseIcon = relativePath + fileName;
            }
            var result = _apiLibraryRepository.AddOrUpdate(obj);
            return this.ToJsonOperateResult();
        }
        [HttpPost]
        public ActionResult ApiLibraryDelete(int[] ids)
        {
            var objs = ids.Select(o => (object)o);
            _apiLibraryRepository.DeletesWithSaveChanges(objs.ToArray());
            return this.ToJsonOperateResult();
        }
        [HttpPost]
        public ActionResult MoveItem(int mode, int id)
        {
            _apiLibraryRepository.MoveItem(mode, id);
            return this.ToJsonOperateResult();
        }
        [HttpPost]
        public ActionResult ApiLibraryState(string ids, short state)
        {
            _apiLibraryRepository.SetState(ids, state);
            return this.ToJsonOperateResult();
        }

        #endregion

        #region 数据迁移
        public ActionResult SaleDataMove()
        {
            return View();
        }
        public ActionResult Import()
        {
            ViewBag.stores = this.ToSelectTitle(_warehouseRepository.GetList(true).Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "请选择");
            var obj = _importSetService.GetByName("SaleOrders");
            return View(obj ?? new ImportSet() { MinRow = 1 });
        }
        [HttpPost]
        public ActionResult Import(ImportSet imp)
        {
            imp.TableName = "SaleOrders";
            var op = _saleImportService.Import(imp, Request.Files, Request["FieldName"], Request["ColumnName"]);
            return this.ToJsonOperateResult(op);
        }
        public ActionResult SureImport()
        {
            var op = _saleImportService.SureImport();
            return this.ToJsonOperateResult(op);
        }
        [HttpPost]
        public ActionResult SaleDataMoveList(string type, string apiTitle, string searchField, string searchText)
        {
            System.Data.DataView dv = null;
            object foots = new List<object>();
            dv = _saleImportService.SaleDataMoveList(type, apiTitle, searchText, searchField, ref foots);
            return this.ToDataGrid(dv == null ? null : dv.ToTable(), dv == null ? 0 : dv.Count, foots);
        }
        [HttpPost]
        public ActionResult GetDataForSearch(string searchText, string searchField)
        {
            var dt = _saleImportService.GetDataFromCache;
            var list = new List<DropdownItem>();
            var drs = dt.Select(searchField + " like '%" + searchText + "%'");
            list = drs.Select(o => o[searchField].ToString()).Distinct().Select(o => new DropdownItem(o)).ToList();
            return this.ToJsonResult(list);
        }
        [HttpPost]
        public void ClearImport()
        {
            _saleImportService.ClearImport();
        }
        #endregion
    }
}
