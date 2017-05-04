using Autofac;
using Qct.Infrastructure.DI;
using Qct.IServices;
using Qct.POS.Api.Retailing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;

namespace Qct.POS.Api.Retailing.Attributes
{
    public class StoreUserAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //从http请求的头里面获取身份验证信息，验证是否是请求发起方的ticket
            var authorization = actionContext.Request.Headers.Authorization;
            var strTicket = authorization?.Parameter;
            if (string.IsNullOrEmpty(strTicket))
            {
                strTicket = HttpContext.Current.Request["token"];
            }

            if (!string.IsNullOrEmpty(strTicket))
            {
                //解密用户ticket,并校验用户名密码是否匹配
                if (ValidateTicket(strTicket))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }
        }

        private bool ValidateTicket(string strTicket)
        {
            IStoreUserService storeUserService = AutofacBootstapper.CurrentContainer.ResolveOptional<IStoreUserService>(
               new NamedParameter("token", strTicket)
               );
            var user = storeUserService.GetLoginCredentials();
            if (user == null) return false;
            var principal = new StoreUserPrincipal(user);
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
            return true;
        }
    }
}