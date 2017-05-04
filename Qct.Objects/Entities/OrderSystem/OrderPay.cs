using Qct.Pay;

namespace Qct.Objects.Entities
{
    public class OrderPay
    {
        /// <summary>
        /// 支付类型
        /// </summary>
        public PayType PayType { get; set; }
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderSn { get; set; }

        /// <summary>
        /// 支付平台支付流水号
        /// </summary>
        public string PaymentPlatformSn { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 收到金额（ReceivedAmount可能大于PayAmount）
        /// </summary>
        public decimal ReceivedAmount { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 找零
        /// </summary>
        public decimal ChangedAmount { get; set; }


    }
}
