using Qct.Domain.CommonObject.User;
using Qct.Domain.Objects.Models;
using System;

namespace Qct.Objects.Entities
{
    public partial class SysStoreUserInfo
    {
        public SysStoreUserInfo()
        {
            SyncItemId = Guid.NewGuid();
        }

        public UserCredentials DoLogin(string storeId, string machineSN, string deviceSN, bool practice, string token)
        {
            LoginDT = DateTime.Now;
            var userAuth = new UserCredentials()
            {
                Account = UserCode,
                IsLogin = true,
                IsPracticed = practice,
                FullName = FullName,
                Token = token,
                UserCode = UserCode,
                CompanyId = CompanyId,
                StoreId = storeId,
                MachineSn = machineSN,
                DeviceSn = deviceSN,
                UserID = UID,
                StoreUserRoles = new StoreUserRole(OperateAuth).GetStoreUserRoles(storeId)
            };
            return userAuth;
        }
    }
}
