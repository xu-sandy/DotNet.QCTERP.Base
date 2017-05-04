using Qct.Domain.CommonObject.User;
using Qct.Infrastructure.Data;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.IRepository
{
    public interface ISysStoreUserInfoRepository : IEFRepository<SysStoreUserInfo>
    {
        /// <summary>
        /// 用账号密码查找门店账号
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>用户信息</returns>
        SysStoreUserInfo FindStoreUser(int companyId, string account, string password);
        /// <summary>
        /// 通过门店用户角色查找用户组
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="storeId">门店Id</param>
        /// <param name="role">角色</param>
        /// <returns>用户组</returns>
        IEnumerable<SysStoreUserInfo> FindStoreUserWithRole(int companyId, string storeId, StoreUserRoleType role);

        SysStoreUserInfo GetByUid(string uid);
        
    }
}
