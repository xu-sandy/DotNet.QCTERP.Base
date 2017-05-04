using Qct.Marketings;
using Qct.Member;
using Qct.Objects.Entities;
using System.Collections.Generic;

namespace Qct.IServices
{
    /// <summary>
    /// 促销服务【此服务决定了促销加载方式，促销运算顺序】
    /// </summary>
    public interface IMarketingService
    {
        /// <summary>
        ///  订单促销匹配
        /// </summary>
        /// <param name="order">购物车或者订单</param>
        /// <param name="marketingStage">促销执行的阶段</param>
        /// <returns>促销结果</returns>
        IEnumerable<IOrderMarketingResult> MatchOrder(IOrder order, OrderMarketingStage marketingStage);
        /// <summary>
        /// 预付款（即储值卡充值）促销匹配
        /// </summary>
        /// <param name="order">购物车或者订单</param>
        /// <returns>促销结果</returns>
        IEnumerable<IMemberPrepaidOrderMarketingResult> MatchMemberPrepaid(IPrepaidOrder order);
    }
}