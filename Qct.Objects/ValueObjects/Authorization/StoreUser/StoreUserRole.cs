using Qct.Domain.CommonObject.User;
using Qct.Objects.ValueObjects;
using System.Collections.Generic;

namespace Qct.Domain.Objects.Models
{
    public class StoreUserRole
    {
        private readonly string authText;
        public StoreUserRole(string authText)
        {
            this.authText = authText;
        }
        /// <summary>
        /// 获取系统配置的用户门店授权信息
        /// </summary>
        /// <param name="storeId">请求门店</param>
        /// <returns>该门店下用户的授权列表</returns>
        public IEnumerable<StoreUserRoleType> GetStoreUserRoles(string storeId)
        {
            List<StoreUserRoleType> storeUserRoles = new List<StoreUserRoleType>();
            if (!string.IsNullOrWhiteSpace(authText))
            {
                var authString = "|" + authText + "|";
                if (authString.Contains(string.Format(ConstValues.StoreAuthFormat, storeId, (int)StoreUserRoleType.Cashier)))
                {
                    storeUserRoles.Add(StoreUserRoleType.Cashier);
                }
                if (authString.Contains(string.Format(ConstValues.StoreAuthFormat, storeId, (int)StoreUserRoleType.DataManager)))
                {
                    storeUserRoles.Add(StoreUserRoleType.DataManager);
                }
                if (authString.Contains(string.Format(ConstValues.StoreAuthFormat, storeId, (int)StoreUserRoleType.ShopManager)))
                {
                    storeUserRoles.Add(StoreUserRoleType.ShopManager);
                }
                if (authString.Contains(string.Format(ConstValues.StoreAuthFormat, storeId, (int)StoreUserRoleType.ShoppingGuide)))
                {
                    storeUserRoles.Add(StoreUserRoleType.ShoppingGuide);
                }
            }
            return storeUserRoles;
        }
    }
}
