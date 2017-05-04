namespace Qct.IServices
{
    /// <summary>
    /// 促销结果运算器
    /// </summary>
    public interface IMarketingResultBuilder
    {
        /// <summary>
        /// 运算促销规则，并生成促销上下文
        /// </summary>
        void BuilderMarketingContext();
        /// <summary>
        /// 根据促销上下文运算促销结果，并生存MarketingResult对象
        /// </summary>
        void BuilderMarketingResult();
    }
}
