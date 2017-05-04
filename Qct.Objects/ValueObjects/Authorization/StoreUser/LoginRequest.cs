namespace Qct.Domain.CommonObject.User
{
    /// <summary>
    /// 登录参数
    /// </summary>
    public class LoginAction
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineSn { get; set; }
        /// <summary>
        /// 设备硬件标识
        /// </summary>
        public string DeviceSn { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 是否为练习模式
        /// </summary>
        public bool Practice { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
