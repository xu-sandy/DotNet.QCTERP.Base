namespace Qct.Domain.CommonObject.User
{
    /// <summary>
    /// 收银员身份凭证
    /// </summary>
    public class CashierCredentials
    {
        /// <summary>
        /// 登录凭证号
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
    }
}
