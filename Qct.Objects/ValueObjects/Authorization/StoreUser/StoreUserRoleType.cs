namespace Qct.Domain.CommonObject.User
{
    /// <summary>
    /// 门店用户权限
    /// </summary>
    public enum StoreUserRoleType
    {
        /// <summary>
        /// 店长
        /// </summary>
        ShopManager = 1,
        /// <summary>
        /// 营业员/导购员
        /// </summary>
        ShoppingGuide = 2,
        /// <summary>
        /// 收银员
        /// </summary>
        Cashier = 3,
        /// <summary>
        /// 数据维护员
        /// </summary>
        DataManager = 4
    }
}
