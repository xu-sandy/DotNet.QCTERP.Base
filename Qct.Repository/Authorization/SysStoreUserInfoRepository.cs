using Qct.Domain.CommonObject.User;
using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.IRepository;
using Qct.Objects.Entities;
using Qct.Persistance.Data;
using System.Collections.Generic;
using System.Linq;
using Qct.Objects.ValueObjects;

namespace Qct.Repository
{
    public class SysStoreUserInfoRepository : EFRepositoryWithIntegerIdEntity<SysStoreUserInfo>, ISysStoreUserInfoRepository
    {
        public SysStoreUserInfoRepository() : base(DataContextFactory.Create<DataContext>())
        { }

        public SysStoreUserInfo GetByUid(string uid)
        {
            return GetEntities().FirstOrDefault(o => o.UID == uid);
        }

        /// <summary>
        /// 用账号密码查找门店账号
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>用户信息</returns>
        public SysStoreUserInfo FindStoreUser(int companyId, string account, string password)
        {
            var user = GetReadOnlyEntities().FirstOrDefault(o => o.CompanyId == companyId && o.LoginPwd == password && o.UserCode == account);
            return user;
        }
        /// <summary>
        /// 通过门店用户角色查找用户组
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="storeId">门店Id</param>
        /// <param name="role">角色</param>
        /// <returns>用户组</returns>
        public IEnumerable<SysStoreUserInfo> FindStoreUserWithRole(int companyId, string storeId, StoreUserRoleType role)
        {
            var roleText = string.Format(ConstValues.StoreAuthFormat, storeId, (int)role);
            IEnumerable<SysStoreUserInfo> result = GetReadOnlyEntities().Where(o => o.CompanyId == companyId && ("|" + o.OperateAuth + "|").Contains(roleText));
            return result;
        }
    }
}
