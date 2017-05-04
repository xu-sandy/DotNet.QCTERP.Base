using Qct.IServices;
using System;
using System.Linq;
using Qct.Domain.CommonObject.User;
using Qct.IRepository;
using Qct.Repository.Exceptions;
using Qct.Domain.Objects;
using Qct.Domain.Objects.Models;
using Qct.ISevices;
using Qct.Objects.ValueObjects;
using Qct.Objects.ValueObjects.Systems;

namespace Qct.Services.Authorization
{
    /// <summary>
    /// 门店用户登录
    /// </summary>
    public class StoreUserService : IStoreUserService
    {
        ISysStoreUserInfoRepository _sysUserRepository;
        int _CompanyId;
        string _StoreId;
        string _Token;
        string _MachineSn;
        string _DeviceSn;
        ICacheService _CacheService;
        IDeviceRepository _deviceRepository;
        /// <summary>
        /// 登录后获取登录信息使用此构造函数
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cacheService"></param>
        public StoreUserService(string token, ICacheService cacheService)
        {
            _Token = token;
            _CacheService = cacheService;

        }
        /// <summary>
        /// 正在登录使用此构造函数
        /// </summary>
        /// <param name="sysUserRepository">用户仓储</param>
        /// <param name="cacheService">缓存服务</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="storeId">门店Id</param>
        /// <param name="machineSn">设备编号</param>
        /// <param name="deviceSn">设备标识</param>
        public StoreUserService(ISysStoreUserInfoRepository sysUserRepository, ICacheService cacheService, IDeviceRepository deviceRepository, int companyId, string storeId, string machineSn, string deviceSn)
        {
            _sysUserRepository = sysUserRepository;
            _CompanyId = companyId;
            _StoreId = storeId;
            _MachineSn = machineSn;
            _DeviceSn = deviceSn;
            _CacheService = cacheService;
            _deviceRepository = deviceRepository;
        }
        /// <summary>
        /// 获取登录凭证
        /// </summary>
        /// <param name="isNotFoundThrowException">未找到是否抛出异常</param>
        /// <returns></returns>
        public UserCredentials GetLoginCredentials(bool isNotFoundThrowException = false)
        {
            if (string.IsNullOrEmpty(_Token) && isNotFoundThrowException)
            {
                throw new UnauthorizedException(ConstValues.LoginCredentialsNotFind);
            }
            else if (string.IsNullOrEmpty(_Token))
            {
                return null;
            }
            else
            {
                var credentials = _CacheService.GetObject<UserCredentials>(string.Format(ConstValues.CacheStoreUserKey, _Token));
                if (isNotFoundThrowException && credentials == null)
                {
                    throw new UnauthorizedException(ConstValues.LoginCredentialsNotFind);
                }
                return credentials;
            }
        }
        /// <summary>
        /// 获取登录凭证号
        /// </summary>
        /// <returns></returns>
        public string GetLoginTokenKey()
        {
            if (string.IsNullOrEmpty(_Token))
            {
                _Token = Guid.NewGuid().ToString("N");
            }
            return _Token;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <param name="practice">是否为练习模式</param>
        /// <returns>用户登录凭证</returns>
        public UserCredentials Login(string account, string password, bool practice)
        {
            var device = _deviceRepository.Get(_CompanyId, _StoreId, _MachineSn);
            if (device == null)
            {
                throw new UnauthorizedException("未能找到设备信息，请先注册设备！");
            }
            if (device.State == DeviceState.Disable)
            {
                throw new UnauthorizedException("设备未通过授权，请到后台启用设备！");
            }
            var user = _sysUserRepository.FindStoreUser(_CompanyId, account, password);
            if (user == null)
            {
                throw new NotFoundUserException(ConstValues.StoreUserVerfyError);
            }
            StoreUserRole storeUserRole = new StoreUserRole(user.OperateAuth);
            var storeUserRoles = storeUserRole.GetStoreUserRoles(_StoreId);
            if (!storeUserRoles.Any(o => o == StoreUserRoleType.Cashier || o == StoreUserRoleType.DataManager))
            {
                throw new UnauthorizedException(ConstValues.LoginAuthorizationFailures);
            }
            var userAuth = user.DoLogin(_StoreId, _MachineSn, _DeviceSn, practice, GetLoginTokenKey());
            _sysUserRepository.SaveChanges();
            _CacheService.SetObject(string.Format(ConstValues.CacheStoreUserKey, userAuth.Token), userAuth, new TimeSpan(24, 0, 0));
            return userAuth;
        }
        /// <summary>
        /// 登出
        /// </summary>
        public void Logout()
        {
            _CacheService.RemoveObject<UserCredentials>(string.Format(ConstValues.CacheStoreUserKey, _Token));
        }
    }
}
