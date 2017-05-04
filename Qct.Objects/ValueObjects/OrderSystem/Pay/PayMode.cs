namespace Qct.Pay
{
    /// <summary>
    /// 支付模式
    /// </summary>
    public enum PayMode
    {
        /// <summary>
        /// 仅记录交易数据信息，需要手动录入支付信息，不支持接口调用第三方支付平台完成支付，即台账，如：现金支付
        /// </summary>
        OnlyRecord = 1,
        /// <summary>
        /// 由服务器实现支付接口，支付全过程在服务器完成。如：微信二维码支付
        /// </summary>
        PayInServer = 2,
        /// <summary>
        /// 由门店收银软件实现支付接口，支付全过程在门店收银台。如：对接POS银联机
        /// </summary>
        PayInClient = 3
    }
}
