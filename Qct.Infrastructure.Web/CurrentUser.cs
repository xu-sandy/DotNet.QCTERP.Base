// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-26
// 描述信息：当前登录用户信息（总部后台）
// --------------------------------------------------

using System;
using System.Web;
using System.Collections.Generic;
using Qct.Objects.Entities.Authorization;
using Qct.Infrastructure.Helpers;

namespace Qct.Infrastructure.Web
{
    /// <summary>
    /// 当前登录用户信息（总部后台）
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// 是否已登录
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                return !string.IsNullOrEmpty(UID);
            }
        }
                
        /// <summary>
        /// 统一登录写入cookie
        /// </summary>
        /// <param name="user">Entity.SysUserInfo 用户信息类</param>
        /// <param name="remember">记住用户和密码，默认false</param>
        public static void Login(SysUserInfo user, bool remember = false)
        {
            Dictionary<string, string> kv = new Dictionary<string, string>();

            kv.Add(key_cid, user.CompanyId.ToString());
            kv.Add(key_uid, user.UID);
            kv.Add(key_uname, user.LoginName);
            kv.Add(key_fname, HttpUtility.UrlEncode(user.FullName));

            kv.Add(key_branchId, user.BranchId.ToString());
            kv.Add(key_bumenId, user.BumenId.ToString());
            kv.Add(key_photo, user.PhotoUrl);

            kv.Add(key_roleId, user.RoleIds);

            CookieHelper.Remove("remuc");
            if (remember)
            {
                //kv.Add("_pwd", user.LoginPwd);
                CookieHelper.Set("remuc", kv, 100, true);//防止退出删除
            }
            CookieHelper.Set(Url.CurDomain,"", uc, kv,1,false);
            var browser= HttpContext.Current.Request.Browser;
            //SysLogService.WriteInfo(string.Format("用户（{0}，{1}）成功登录系统！浏览器:{2},版本:{3}", user.LoginName, user.FullName,browser.Browser,browser.Version),LogType.登录,LogModule.其他);
        }

        private const string uc = "storeuc";
        private const string key_pwd = "_storepwd";

        /// <summary>
        /// 安全退出
        /// </summary>
        public static void Exit()
        {
            if (IsLogin)
            {
                //SysLogService.WriteInfo(string.Format("用户（{0}，{1}）成功退出系统！", UserName, FullName), LogType.退出, LogModule.其他);
                CookieHelper.SetExpires(Url.CurDomain,"/",uc);
            }
        }

        public static string PWD
        {
            get
            {
                return CookieHelper.Get(uc, key_pwd);
            }
        }

        /// <summary>
        /// 用户UID
        /// </summary>
        public static string UID
        {
            get
            {
                return CookieHelper.Get(uc, key_uid);
            }
        }
        private const string key_uid = "_u";

        /// <summary>
        ///  CID
        /// </summary>
        public static string CID
        {
            get
            {
                return CookieHelper.Get(uc, key_cid);
            }
        }
        private const string key_cid = "_cid";

        /// <summary>
        /// 登录名称
        /// </summary>
        public static string UserName
        {
            get
            {
                return HttpUtility.UrlDecode(CookieHelper.Get(uc, key_uname));
            }
        }
        private const string key_uname = "_uname";

        /// <summary>
        /// 用户姓名
        /// </summary>
        public static string FullName
        {
            get
            {
                return HttpUtility.UrlDecode(CookieHelper.Get(uc, key_fname));
            }
        }
        private const string key_fname = "_fname";

        /// <summary>
        /// 隶属机构ID
        /// </summary>
        public static int BranchId
        {
            get
            {
                string id = CookieHelper.Get(uc, key_branchId);
                return !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : -1;
            }
        }
        private const string key_branchId = "_branchId";

        /// <summary>
        /// 隶属部门ID
        /// </summary>
        public static int BumenId
        {
            get
            {
                string id = CookieHelper.Get(uc, key_bumenId);
                return !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : -1;
            }
        }
        private const string key_bumenId = "_bumenId";

        /// <summary>
        /// 头像URL
        /// </summary>
        public static string Photo
        {
            get
            {
                return CookieHelper.Get(uc, key_photo);
            }
        }
        private const string key_photo = "_photo";

        /// <summary>
        /// 角色ID
        /// </summary>
        public static string RoleIds
        {
            get
            {
                return CookieHelper.Get(uc, key_roleId);
            }
        }
        private const string key_roleId = "_roleId";

        
        /// <summary>
        /// 判断用户是否有对应的访问权限
        /// </summary>
        /// <param name="limitId"></param>
        /// <returns></returns>
        public static bool HasPermiss(int limitId)
        {
            var result = false;
            //var roleBLL = new SysRoleBLL();//测试
            //Dictionary<int, int> objs = null;
            //if (string.IsNullOrWhiteSpace(CurrentUser.RoleIds))
            //    objs = roleBLL.GetRoleLimitsByUId(CurrentUser.UID);//测试
            //else
            //    objs = roleBLL.GetRoleLimitsByRoleId(CurrentUser.RoleIds);//门店根据角色

            //if (objs == null || objs.Count<=0) {
            //    //重新初始化当前登录用户权限对象并设置全局缓存
            //    //SettingLimits(CurrentUser.UID);
            //    objs = roleBLL.GetRoleLimitsByUId(CurrentUser.UID);
            //}
            //if (objs != null)
            //{
            //    var curLimits = (Dictionary<int, int>)objs;
            //    //判断当前用户的权限是否包含数据权限
            //    if (curLimits.ContainsKey(limitId))
            //    {
            //        result = true;
            //        return result;
            //    }
            //}
            return result;
        }
        
    }
}
