using Qct.Marketings;
using System.Collections.Generic;
using System.Linq;

namespace Qct.IRepository
{
    /// <summary>
    /// 促销规则仓储接口
    /// </summary>
    public interface IMarketingRuleRepository
    {
        /// <summary>
        /// 获取当前生效的促销规则
        /// </summary>
        /// <returns></returns>
        IQueryable<MarketingRule> GetCurrentMarketingRules();
        /// <summary>
        /// 获取特价规则
        /// </summary>
        /// <returns></returns>
        IEnumerable<MarketingRule> GetSpecialPriceMarketingRules();
        /// <summary>
        /// 获取折扣促销
        /// </summary>
        /// <returns></returns>
        IEnumerable<MarketingRule> GetDiscountMarketingRules();
        /// <summary>
        /// 获取赠品促销
        /// </summary>
        /// <returns></returns>
        IEnumerable<MarketingRule> GetGiftMarketingRules();
        /// <summary>
        /// 获取积分促销
        /// </summary>
        /// <returns></returns>
        IEnumerable<MarketingRule> GetIntegralMarketingRules();
        /// <summary>
        /// 获取换购促销
        /// </summary>
        /// <returns></returns>
        IEnumerable<MarketingRule> GetTradeInMarketingRules();
        /// <summary>
        /// 获取代金券促销
        /// </summary>
        /// <returns></returns>
        IEnumerable<MarketingRule> GetCouponMarketingRules();
        /// <summary>
        /// 获取减现促销
        /// </summary>
        /// <returns></returns>
        IEnumerable<MarketingRule> GetReducedCashMarketingRules();

    }
}
