using Qct.Infrastructure.Data;
using Qct.Objects.Entities;
using System.Collections.Generic;

namespace Qct.Marketings
{
    public interface IOrderMarketingResult
    {
        /// <summary>
        /// 订单促销上下文
        /// </summary>
        OrderMarketingContext MarketingContext { get; }
        /// <summary>
        /// 促销结果描述信息
        /// </summary>
        string MarketingRuleDescription { get; set; }

        /// <summary>
        /// 根据促销结果获取减现金额，如未减现则返回0
        /// </summary>
        decimal ReducedCash { get; set; }
        /// <summary>
        /// 赠品方案选项
        /// </summary>
        IEnumerable<GiftOption> GiftOptions { get; set; }
        /// <summary>
        /// 换购方案选项
        /// </summary>
        IEnumerable<TradeInOption> TradeInOptions { get; set; }

        /// <summary>
        /// 将结果保存到数据库
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="order"></param>
        void SaveResult(IEntityFrameworkUnitOfWork unitOfWork, Order order);
        /// <summary>
        /// 根据赠品、换购结果生成订单项目，当无新订单项目生成时，返回null;
        /// </summary>
        /// <returns></returns>
        IEnumerable<OrderItem> GetGiftsOrTradeInItems();


    }
}
