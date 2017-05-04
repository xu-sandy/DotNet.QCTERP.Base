
using Qct.Objects.Entities;

namespace Qct.IServices
{
    /// <summary>
    /// 门店用户服务接口
    /// </summary>
    public interface ISysUserService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        SysUserInfo Login(string account, string password,ref string message, bool remember = false);
        ///// <summary>
        ///// 获取当前用户登录凭证
        ///// </summary>
        ///// <returns>用户登录凭证</returns>
        //UserCredentials GetCurrentUser();
    }
}
