using Qct.Domain.CommonObject.User;

namespace Qct.IServices
{
    /// <summary>
    /// 门店用户服务接口
    /// </summary>
    public interface IStoreUserService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <param name="practice">是否进行练习</param>
        UserCredentials Login(string account, string password, bool practice);
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        void Logout();
        /// <summary>
        /// 获取登录凭证
        /// </summary>
        /// <param name="isNotFoundThrowException">未找到时是否抛出异常</param>
        /// <returns>登录凭证</returns>
        UserCredentials GetLoginCredentials(bool isNotFoundThrowException = false);
        /// <summary>
        /// 获取登录凭证Key
        /// </summary>
        /// <returns>登录凭证Key</returns>
        string GetLoginTokenKey();
    }
}
