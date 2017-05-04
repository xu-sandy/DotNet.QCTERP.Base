using Qct.Domain.CommonObject.User;
using Qct.Infrastructure.Security;
using Qct.IRepository;
using Qct.IServices;
using Qct.Services.Exceptions;
using Qct.Repository.Pos.Common;
using Qct.Repository.Pos;
using System;

namespace Qct.Services
{
    /// <summary>
    /// 门店POS在线用户管理
    /// </summary>
    public class StoreUserService : IStoreUserService
    {
        private static UserCredentials loginUserInformaction;
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <param name="practice">是否进行练习</param>
        public UserCredentials Login(string account, string password, bool practice)
        {
            IPosDeviceRepository posDeviceRepository = new PosDeviceRepository();
            var deviceInfo = posDeviceRepository.Get(true);

            var userLogin = new LoginAction()
            {
                Account = account,
                Password = MD5.EncryptOutputHex(password, isUpper: false),
                CompanyId = deviceInfo.CompanyId,
                DeviceSn = deviceInfo.DeviceSn,
                MachineSn = deviceInfo.MachineSn,
                Practice = practice,
                StoreId = deviceInfo.StoreId
            };
            var result = POSRestClient.Post<LoginAction, UserCredentials>("api/Session", userLogin);
            if (result.Successed)
            {
                loginUserInformaction = result.Data;
                POSRestClient.SetToken(loginUserInformaction.Token);
                return loginUserInformaction;
            }
            else
            {
                throw new NetException(result.Message);
            }
        }
        /// <summary>
        /// 获取登录凭证
        /// </summary>
        /// <param name="isNotFoundThrowException">未找到时是否抛出异常</param>
        /// <returns>登录凭证</returns>
        public UserCredentials GetLoginCredentials(bool isNotFoundThrowException = false)
        {
            if (isNotFoundThrowException && loginUserInformaction == null)
                throw new UnauthorizedException("未找到用户登录信息，请登录后进行操作！");
            return loginUserInformaction;
        }
        /// <summary>
        /// 获取登录凭证Key
        /// </summary>
        /// <returns>登录凭证Key</returns>
        public string GetLoginTokenKey()
        {
            return GetLoginCredentials(true).Token;
        }
        /// <summary>
        /// POS 登出
        /// </summary>
        public void Logout()
        {
            var result = POSRestClient.Delete<object>("api/Session");
            if (!result.Successed)
            {
                throw new NetException(result.Message);
            }
        }
    }
}
