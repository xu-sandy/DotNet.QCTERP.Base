using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Qct.Infrastructure.Web.Extensions;
using Qct.IRepository;

namespace Qct.ERP.Retailing.Controllers
{
    /// <summary>
    /// 公共方法统一调用
    /// </summary>
    public class CommonController : Controller
    {
        private readonly ISysDepartmentRepository _sysDepartmentRespository;
        private readonly ISysRolesRepository _sysRoleRespository;
        private readonly ISysDictionaryRepository _sysDataDictionaryRespository;
        private readonly ISysUserRespository _userRespository;
        public CommonController(ISysUserRespository userRespository, ISysDepartmentRepository sysDepartmentRespository, ISysRolesRepository sysRoleRespository, ISysDictionaryRepository sysDataDictionaryRespository)
        {
            _sysDepartmentRespository = sysDepartmentRespository;
            _sysRoleRespository = sysRoleRespository;
            _sysDataDictionaryRespository = sysDataDictionaryRespository;
            _userRespository = userRespository;
        }
        //
        // GET: /Common/
        #region 系统管理
        /// <summary>
        /// 通用-获取系统用户下拉数据
        /// </summary>
        /// <param name="selecttype">全部：selecttype=1 请选择selecttype=0</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUsersEUIDropdown(int selecttype = 0,string uid="")
        {
            var datas= this.ToSelectTitle( _userRespository.GetList(selecttype == 1,uid).Select(o => new SelectListItem() { Value = o.UID, Text = o.FullName }), 
                emptyTitle: (selecttype == 1 ? "全部" : "请选择"));
            return this.ToJsonResult(datas);
        }
        /// <summary>
        /// 通用-获取机构下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOrgsEasyuiDropdown(int selecttype = 0)
        {
            var datas = this.ToSelectTitle(_sysDepartmentRespository.GetListByType(1).Select(o => new SelectListItem() { Value = o.DepId.ToString(), Text = o.Title }), emptyTitle: (selecttype == 1 ? "全部" : "请选择"));
            return this.ToJsonResult(datas);
        }
        /// <summary>
        /// 通用-获取部门下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDepsEasyuiDropdown(int? pDepId, int selecttype = 0)
        {
            var datas = this.ToSelectTitle(_sysDepartmentRespository.GetListByPDepId(pDepId.GetValueOrDefault()).Select(o => new SelectListItem() { Value = o.DepId.ToString(), Text = o.Title }), emptyTitle: (selecttype == 1 ? "全部" : "请选择"));
            return this.ToJsonResult(datas);
        }
        /// <summary>
        /// 通用-获取角色下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRolesEasyuiDropdown(int selecttype = 0)
        {
            var datas = this.ToSelectTitle(_sysRoleRespository.GetRoleList().Select(o => new SelectListItem() { Value = o.RoleId.ToString(), Text = o.Title }), emptyTitle: (selecttype == 1 ? "全部" : "请选择"));
            return this.ToJsonResult(datas);
        }
        /// <summary>
        /// 通用-根据父级类别下的子数据字典项下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDicsEasyuiDropdown(int psn, int selecttype = 0)
        {
            var datas = this.ToSelectTitle(_sysDataDictionaryRespository.GetItemsByDicpsn(psn).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: (selecttype == 1 ? "全部" : "请选择"));
            return this.ToJsonResult(datas);
        }
        #endregion

        ///// <summary>
        ///// 通用-获取品牌下拉数据
        ///// </summary>
        ///// <param name="showAll">是否显示所有项（涉及状态）</param>
        ///// <param name="emptyTitle">空值项显示文本</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetBrandsList(bool showAll = false, string emptyTitle = "请选择")
        //{
        //    var data = ProductBrandService.GetList(showAll).Select(o => new DropdownItem(o.BrandSN.ToString(), o.Title)).ToList();
        //    data.Insert(0, new DropdownItem("", Server.UrlDecode(emptyTitle), true));
        //    return new JsonNetResult(data);
        //}
        //public ActionResult GetSuppliersDropdown(string emptyTitle,string value="")
        //{
        //    var list = SupplierService.GetList().Where(o => o.BusinessType == 1).Select(o => new DropdownItem() { Value = o.Id, Text = o.FullTitle }).ToList();
        //    if (!string.IsNullOrWhiteSpace(emptyTitle) && string.IsNullOrWhiteSpace(value))
        //        list.Insert(0, new DropdownItem() { Value = "", Text = emptyTitle, IsSelected = true });
        //    else if(list.Any())
        //    {
        //        var obj = list.FirstOrDefault(o => o.Value == value);
        //        if (obj != null)
        //            obj.IsSelected = true;
        //        else if(emptyTitle!=null && !emptyTitle.StartsWith(" "))
        //            list.FirstOrDefault().IsSelected = true;
        //    }
        //    return new JsonNetResult(list);
        //}
    }
}
