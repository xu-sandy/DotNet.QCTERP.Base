using Qct.Marketings;

namespace Qct.IServices
{
    /// <summary>
    /// 会员预付款（会员充值）促销结果运算器
    /// </summary>
    public interface IMemberPrepaidOrderMarketingResultBuilder : IMarketingResultBuilder
    {
        /// <summary>
        /// 获取会员预付款（会员充值）促销结果
        /// </summary>
        /// <returns></returns>
        IMemberPrepaidOrderMarketingResult GetResult();
    }
}
