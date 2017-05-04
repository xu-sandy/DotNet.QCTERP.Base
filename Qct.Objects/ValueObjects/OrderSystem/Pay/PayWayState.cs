namespace Qct.Pay
{
    /// <summary>
    /// 支付项目信息（包含支付信息及支付授权状态）
    /// </summary>
    public class PayWayState
    {
        /// <summary>
        /// 支付类型（对应原v3版本的apicode）
        /// </summary>
        public PayType PayType { get; set; }

        /// <summary>
        /// 支付名称或者标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 可用图标
        /// </summary>
        public string EnableIcon { get; set; }

        /// <summary>
        /// 不可用图标
        /// </summary>
        public string DisableIcon { get; set; }

        /// <summary>
        /// 支付是否授权【门店级别的授权】
        /// </summary>
        public bool State { get; set; }
    }
}
