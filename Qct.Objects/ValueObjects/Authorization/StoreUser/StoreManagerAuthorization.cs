namespace Qct.Domain.CommonObject.User
{
    /// <summary>
    /// 店长授权
    /// </summary>
    public class StoreManagerAuthorization
    {
        /// <summary>
        /// 当前收银员凭证
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
