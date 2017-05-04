using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.ValueObjects
{
    public static class ConstValues
    {
        public const string StoredValueCardPayPasswordError = "您输入的支付密码错误！";
        public const string StoredValueCardPayBalanceNotEnough = "卡内余额不足，当前余额{0:N2}元！";
        public const string StoredValueCardPayCardNotFind = "未能找到储值卡{0}！";
        public const string StoredValueCardPayCardState = "储值卡{0}{1}！";
        public const string StoredValueCardPayCardUserNotFind = "支付失败，未能找到该卡所绑定的会员信息！";
        public const string StoreAuthFormat = "|{0},{1}|";
        public const string CacheStoreUserKey = "{0}.StoreUser";
        public const string StoreUserVerfyError = "您输入的账号或者密码错误！";
        public const string LoginAuthorizationFailures = "非销售员或数据维护员不允许登录收银系统！";
        public const string LoginCredentialsNotFind = "未找到登录凭证，请重新登录系统！";
        public const string StoreManagerAuthorizationFailures = "授权失败，请店长输入正确的授权密码！";
        public const string StoreUserSecondVerfyError = "身份验证失败，请重新输入！";
        public const string LoginCredentialsDisable = "您的登录凭证已经失效，请重新登录！";
        public const string DESKEY = "QCT(全城淘)ERP";
    }
}
