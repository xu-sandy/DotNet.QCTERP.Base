namespace Qct.Pay
{
    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PayType
    {
        /// <summary>
        /// 现金支付
        /// </summary>
        CashPay = 11,
        /// <summary>
        /// 银联支付（台账）
        /// </summary>
        UnionPay = 12,
        /// <summary>
        /// 银联支付(联机)
        /// </summary>
        UnionPayCTPOSM = 13,

        //--14 尚未使用

        /// <summary>
        /// 代金券
        /// </summary>
        CashCoupon = 15,
        /// <summary>
        /// 储值卡
        /// </summary>
        StoredValueCard = 16,
        /// <summary>
        /// 融合支付-动态二维码
        /// </summary>
        RongHeDynamicQRCode = 17,
    }
}
