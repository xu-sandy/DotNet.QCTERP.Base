namespace Qct.Pay
{
    /// <summary>
    /// 储值卡支付结果
    /// </summary>
    public class StoredValueCardPayResult
    {
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardSn { get; set; }
        /// <summary>
        /// 持卡人姓名
        /// </summary>
        public string CardholderName { get; set; }
        /// <summary>
        /// 持卡人移动电话
        /// </summary>
        public string CardholderPhone { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PaymentAmount { get; set; }
    }
}
