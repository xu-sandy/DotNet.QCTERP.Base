using Qct.Domain.CommonObject.User;
using Qct.Infrastructure.DI;
using System.Web.Http;
using Autofac;
using Qct.IServices;
using System.Threading;
using System.Web;
using Qct.POS.Api.Retailing.Models;
using Qct.POS.Api.Retailing.Attributes;
using System;

namespace Qct.POS.Api.Retailing.Controllers
{
    /// <summary>
    /// POS登录会话管理
    /// </summary>
    [StoreUserAuthorize]
    public class SessionController : ApiController
    {
        /// <summary>
        /// POS登录
        /// </summary>
        /// <param name="loginAction">登录参数</param>
        /// <returns>登录凭证</returns>
        [AllowAnonymous]
        public UserCredentials Post([FromBody]LoginAction loginAction)
        {
            IStoreUserService storeUserService = AutofacBootstapper.CurrentContainer.ResolveOptional<IStoreUserService>(
                new NamedParameter("companyId", loginAction.CompanyId),
                new NamedParameter("storeId", loginAction.StoreId),
                new NamedParameter("machineSn", loginAction.MachineSn),
                new NamedParameter("deviceSn", loginAction.DeviceSn)
                );
            var result = storeUserService.Login(loginAction.Account, loginAction.Password, loginAction.Practice);
            var principal = new StoreUserPrincipal(result);
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
            return result;
        }
        /// <summary>
        /// POS登出
        /// </summary>
        public void Delete()
        {
            var user = (StoreUserPrincipal)Thread.CurrentPrincipal;
            IStoreUserService storeUserService = AutofacBootstapper.CurrentContainer.ResolveOptional<IStoreUserService>(
             new NamedParameter("token", user.UserCredentials.Token)
             );
            storeUserService.Logout();
        }
    }
}
