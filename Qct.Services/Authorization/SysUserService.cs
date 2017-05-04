using Qct.Infrastructure.Data;
using Qct.IRepository;
using Qct.IServices;
using Qct.Objects.Entities;
using Qct.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qct.Domain.CommonObject.User;
using Qct.Infrastructure.Helpers;
using System.Web;
using Qct.Infrastructure.Json;
using Qct.Objects.ValueObjects;
using Qct.Infrastructure.Log;

namespace Qct.Services
{
    public class SysUserService : ISysUserService
    {
        readonly ISysUserRespository _userRepository = null;
        public SysUserService(ISysUserRespository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// Cookie用户登录信息Id
        /// </summary>
        private const string uc = "erpuc";

        public SysUserInfo Login(string account, string password, ref string message, bool remember = false)
        {
            var user = _userRepository.Login(account, password);
            if (user == null)
                message = "帐户或密码输入不正确!";
            else if (user.Status != 1)
                message = "该用户状态不正常!";
            else
                SetUserCredentials(user, remember);
            return user;
        }

        public static UserCredentials GetCurrentUser()
        {
            var json = HttpUtility.UrlDecode(CookieHelper.Get(uc));
            if (!string.IsNullOrWhiteSpace(json))
            {
                var user= JsonHelper.ToObject<UserCredentials>(json);
                if (user != null) return user;
            }
            return null;
        }
        public static UserCredentials CurrentUser {
            get {
                var user= GetCurrentUser();
                if (user != null) return user;
                throw new Exception("请重新登陆！");
            }
        }
        public static bool IsLogin
        {
            get
            {
                var user = GetCurrentUser();
                return user != null && !string.IsNullOrWhiteSpace(user.UserID);
            }
        }
        public static void Exit()
        {
            if (IsLogin)
            {
                var user = CurrentUser;
                LoggerFactory.CreateWithSave<SysLog>(new SysLog(user.CompanyId, user.UserID).WriteLogout(string.Format("用户（{0}，{1}）成功退出系统！", user.Account, user.FullName), LogModule.其他));

                CookieHelper.SetExpires(Url.CurDomain, "/", uc);
            }
        }
        /// <summary>
        /// 判断用户是否有对应的访问权限
        /// </summary>
        /// <param name="limitId"></param>
        /// <returns></returns>
        public static bool HasPermiss(int limitId)
        {
            return true;
        }
            /// <summary>
            /// 设置用户登录信息到Cookie
            /// </summary>
            /// <param name="user">用户信息</param>
            /// <param name="remember">是否记住账号</param>
            private void SetUserCredentials(SysUserInfo user, bool remember = false)
        {
            UserCredentials userCredentialsc = new UserCredentials()
            {
                Account = user.LoginName,
                BranchId = user.BranchId.GetValueOrDefault(),
                BumenId = user.BumenId.GetValueOrDefault(),
                CompanyId = user.CompanyId,
                UserCode = user.UserCode,
                UserID = user.UID,
                FullName = user.FullName,
                ErpUserRoles = user.RoleIds,
                Photo = user.PhotoUrl,
                IsLogin = true,
                IsPracticed = false,
                Token = Guid.NewGuid().ToString("N")
            };
            var json = JsonHelper.ToJson(userCredentialsc);
            var accountJson = JsonHelper.ToJson(
                new
                {
                    CompanyId = user.CompanyId,
                    Account = user.LoginName
                });

            CookieHelper.Remove("remuc");

            if (remember)//只记住账号不记住密码
            {
                CookieHelper.Set(Url.CurDomain, "", "remuc", HttpUtility.UrlEncode(accountJson), 100, true);//防止退出删除
            }
            CookieHelper.Set(Url.CurDomain, "", uc,HttpUtility.UrlEncode(json), 1, false);
            var browser = HttpContext.Current.Request.Browser;
            LoggerFactory.CreateWithSave<SysLog>(new SysLog(user.CompanyId, user.UID).WriteLogin(string.Format("用户（{0}，{1}）成功登录系统！浏览器:{2},版本:{3}", user.LoginName, user.FullName, browser.Browser, browser.Version), LogModule.其他));

        }
    }
}
