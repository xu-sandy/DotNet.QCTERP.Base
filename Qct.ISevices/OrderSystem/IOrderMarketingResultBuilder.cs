using Qct.Marketings;

namespace Qct.IServices
{
    /// <summary>
    /// 销售单促销结果运算器
    /// </summary>
    public interface IOrderMarketingResultBuilder : IMarketingResultBuilder
    {
        /// <summary>
        /// 获取订单促销结果
        /// </summary>
        /// <returns></returns>
        IOrderMarketingResult GetResult();
    }
}
