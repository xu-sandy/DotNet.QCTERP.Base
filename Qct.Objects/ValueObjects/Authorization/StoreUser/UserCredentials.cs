using System.Collections.Generic;

namespace Qct.Domain.CommonObject.User
{
    /// <summary>
    /// 用户登录凭证
    /// </summary>
    public class UserCredentials
    {
        /// <summary>
        /// 是否已登录
        /// </summary>
        public bool IsLogin { get; set; }
        /// <summary>
        /// 是否为练习登录
        /// </summary>
        public bool IsPracticed { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 门店Id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineSn { get; set; }
        /// <summary>
        /// 设备唯一标识
        /// </summary>
        public string DeviceSn { get; set; }
        /// <summary>
        /// 登录凭证
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 用户代码
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 用户隶属机构
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// 用户隶属部门
        /// </summary>
        public int BumenId { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// ERP拥有角色
        /// </summary>
        public string ErpUserRoles { get; set; }
        /// <summary>
        /// 用户授权
        /// </summary>
        public IEnumerable<StoreUserRoleType> StoreUserRoles { get; set; }
    }
}
