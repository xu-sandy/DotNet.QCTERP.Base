using System.Web.Mvc;
using System;
using Qct.Infrastructure.Helpers;
using Qct.Infrastructure.Security;
using Qct.Objects.Entities;
using Qct.IServices;
using Qct.Services;
using Qct.Infrastructure.Web;
using Qct.Infrastructure.Extensions;
using Qct.Objects.ValueObjects;

namespace Qct.ERP.Retailing.Controllers
{
    public class AccountController : Controller
    {
        readonly ISysUserService _userService=null;
        public AccountController(ISysUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Login()
        {
            //if (SysUserService.IsLogin)
            //{
            //    //已登录，则直接进入主界面
            //    return Redirect(Url.Action("Index", "Home"));
            //}
            //1为单商户版本，其他值为多商户版本
            string ver = ConfigHelper.GetAppSettings("ver");
            //单商户版本
            if (ver == "1")
            {
                var user = new UserLoginModel();
                if (CookieHelper.IsExist("remuc"))
                {
                    user.UserName = CookieHelper.Get("remuc", "_uname");
                    user.UserPwd = CookieHelper.Get("remuc", "_pwd");
                    user.RememberMe = true;
                }
                return View(user);
            }
            //多商户版本
            else
            {
                return Logins();
            }


        }

        [HttpPost]
        public ActionResult Login(UserLoginModel user)
        {

            //1为单商户版本，其他值为多商户版本
            string ver = ConfigHelper.GetAppSettings("ver");
            //单商户版本
            if (ver == "1")
            {
                if (!ModelState.IsValid) return View(user);
                //var op = Authorize.LoinValidator(CommonService.CompanyId);
                //if (!op.Successed)
                //{
                //    ViewBag.msg = op.Message;
                //    return View(user);
                //}
                string message = "";
                var pwd = MD5.EncryptOutputHex(user.UserPwd);
                var obj = _userService.Login(user.UserName, pwd, ref message,user.RememberMe);
                if (message!="")
                {
                    ViewBag.msg = message;
                    return View(user);
                }
                return Redirect(Url.Action("Index", "Home"));
            }
            //多商户版本
            else
            {
                return Logins(user);
            }

        }

        #region 个人信息
        public ActionResult UserInfo()
        {
            SysUserInfo model = null;
            try
            {
                //var userBll = new Pharos.Sys.BLL.SysUserInfoBLL();
                //model = userBll.GetModelByUID(Sys.SysUserService.UID);
                //ViewBag.BumenTitle = new Pharos.Sys.BLL.SysDepartmentBLL().GetModelByDepId(model.BumenId.GetValueOrDefault()).Title;
                //ViewBag.PositionTitle = new Pharos.Sys.BLL.SysDataDictionaryBLL().GetExtModelByDicSN(model.PositionId.GetValueOrDefault()).Title;
                //var roleBLL = new Pharos.Sys.BLL.SysRoleBLL();
                //var roleIdArray = model.RoleIds.Split(',');SysUserService.RoleIds
                //var roleTitle = string.Empty;
                //var roleStr = string.Empty;
                //foreach (var item in roleIdArray)
                //{
                //    roleTitle = roleBLL.GetModelByRoleId(int.Parse(item)).Title;
                //    if (string.IsNullOrEmpty(roleStr)) roleStr = roleTitle;
                //    else roleStr += "、" + roleTitle;
                //}
                //ViewBag.RoleStr = roleStr;
            }
            catch (Exception ex)
            {
                //new LogEngine().WriteError(ex);
            }
            return View();
        }
        [HttpPost]
        public ActionResult UserInfo(int Id, string LoginPwd)
        {
            //var userBLL = new Pharos.Sys.BLL.SysUserInfoBLL();
            //var model = userBLL.GetModelByUID(SysUserService.UID);
            //model.LoginPwd = LoginPwd;
            //var result = userBLL.UpdateUser(model);
            //return Content(result.ToJson());
            return null;
        }
        #endregion

        #region 多商户登录
        //多商户登录
        public ActionResult Logins()
        {
            //cid是否只读
            int isReadOnly = 0;
            //登录logo
            string logo = "/Content/themes/mytheme/default/images/login/logo.png";
            //电话
            string phone = ConfigHelper.GetAppSettings("phone");

            var user = new UserLoginModel();

            //二级域名
            string d = "";

            //二级域名
            string dom = "";
            if (!RouteData.Values["dom"].IsNullOrEmpty())
            {
                dom = RouteData.Values["dom"].ToString();
            }
            //一级域名
            string d1 = "";
            if (!RouteData.Values["d1"].IsNullOrEmpty())
            {
                d1 = RouteData.Values["d1"].ToString();
            }
            //顶级域名
            string d0 = "";
            if (!RouteData.Values["d0"].IsNullOrEmpty())
            {
                d0 = RouteData.Values["d0"].ToString();
            }

            if (!d0.IsNullOrEmpty())
            {
                if (!dom.IsNullOrEmpty())
                {
                    d = dom;
                }
            }

            //localhost访问、ip访问
            if ((dom.ToLower().Trim() == "localhost") || (dom.IsNullOrEmpty() && d1.IsNullOrEmpty() && d0.IsNullOrEmpty()))
            {
                //获取cookie
                user = setUserLogin(0, user);
            }
            //域名访问
            else
            {
                //API的CID
                int cID = LoginDns.GetCID(d);
                
                //请求API发生错误
                if (cID == -2)
                {
                    return error("");
                }
                //输入的二级域名是空
                else if (cID == -1)
                {
                    //return noBusiness();
                    Response.Redirect("http://www." + dom + "." + d1);
                    return null;
                }
                //输入的域名不存在商户
                else if (cID == 0)
                {
                    return noBusiness();
                }
                //输入的域名是保留二级域名
                else if (cID == -3)
                {
                    //在crm里面
                    if (d.ToLower() == "erp")
                    {
                        //获取cookie
                        user = setUserLogin(0, user);
                    }
                    //不在crm里面
                    else
                    {
                        return noBusiness();
                    }
                }
                //输入的域名存在商户
                else if (cID > 0)
                {
                    //var obj = UserInfoService.Find(o => o.CompanyId == cID);
                    ////CID在目前项目不存在
                    //if (obj == null)
                    //{
                    //    return noUser(cID.ToString());
                    //}
                    //else
                    //{
                        isReadOnly = 1;
                        user.CID = cID;
                        //SysWebSettingBLL bll = new SysWebSettingBLL();
                        //string lg = bll.getLogo(cID);
                        //if (!lg.IsNullOrEmpty())
                        //{
                        //    logo = lg;
                        //}
                        //获取cookie
                        user = setUserLogin(cID, user);
                    //}
                }
            }

            //cid是否只读
            ViewBag.isR = isReadOnly;
            //登录logo
            ViewBag.logo = logo;
            //电话
            ViewBag.phone = phone;

            return View("Logins", user);
        }
        //多商户登录
        [HttpPost]
        public ActionResult Logins(UserLoginModel user)
        {
            //cid是否只读
            ViewBag.isR = user.IsReadOnly;
            //登录logo
            ViewBag.logo = user.Logo;
            //电话
            string phone = ConfigHelper.GetAppSettings("phone");
            ViewBag.phone = phone;

            if (!ModelState.IsValid) return View(user);
            //var op = Authorize.LoinValidator(CommonService.CompanyId);
            //if (!op.Successed)
            //{
            //    ViewBag.msg = op.Message;
            //    return View(user);
            //}
            string message = "";
            var pwd = MD5.EncryptOutputHex(user.UserPwd,isUpper:false);
            var obj =_userService.Login(user.UserName, pwd, ref message, user.RememberMe);
            if (message != "")
            {
                ViewBag.msg = message;
                return View(user);
            }
            return Redirect(Url.Action("Index", "Home"));
        }
        //发生错误
        public ActionResult error(string msg)
        {
            if (msg.IsNullOrEmpty())
            {
                ViewBag.Message = "发生错误，请稍后再访问";
            }
            else
            {
                ViewBag.Message = msg;
            }
            return View("error");
        }
        public ActionResult noBusiness()
        {
            ViewBag.Message = "无法访问，请检查网址是否正确或页面是否存在！";
            return View("noBusiness");
        }
        public ActionResult noUser(string cid)
        {
            ViewBag.cid = cid;
            return View("noUser");
        }


        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="cID"></param>
        /// <param name="u"></param>
        public UserLoginModel setUserLogin(int cID, UserLoginModel user)
        {
            if (CookieHelper.IsExist("remuc"))
            {
                //cookie的CID
                string cid = CookieHelper.Get("remuc", "_cid");
                if (cid.IsNullOrEmpty())
                {
                    cid = "0";
                }

                //输入的域名存在商户
                if (cID > 0)
                {
                    //API的CID等于cookie的CID
                    if (cid == cID.ToString())
                    {
                        user.CID = Convert.ToInt32(cid);
                        user.UserName = CookieHelper.Get("remuc", "_uname");
                        user.UserPwd = CookieHelper.Get("remuc", "_pwd");
                        user.RememberMe = true;
                    }
                }
                //localhost访问、ip访问、保留二级域名访问
                else
                {
                    user.CID = Convert.ToInt32(cid);
                    user.UserName = CookieHelper.Get("remuc", "_uname");
                    user.UserPwd = CookieHelper.Get("remuc", "_pwd");
                    user.RememberMe = true;
                }

            }
            return user;
        }

        #endregion


    }

}