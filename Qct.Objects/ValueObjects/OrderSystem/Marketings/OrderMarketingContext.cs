using Qct.Objects.Entities;
using System.Collections.Generic;

namespace Qct.Marketings
{
    /// <summary>
    /// 促销动作记录
    /// </summary>
    public class OrderMarketingContext : IMarketingContext
    {
        /// <summary>
        /// 影响范围
        /// </summary>
        public IEnumerable<IOrderItem> EffectiveRanges { get; set; }
        /// <summary>
        /// 促销规则
        /// </summary>
        public MarketingRule MarketingRule { get; set; }
    }
}
