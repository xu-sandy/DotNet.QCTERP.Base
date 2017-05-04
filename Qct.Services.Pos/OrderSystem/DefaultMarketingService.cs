using System.Collections.Generic;
using System.Threading.Tasks;
using Qct.Repository;
using Qct.Services.Builders;
using Qct.Member;
using System;
using Qct.IServices;
using Qct.IRepository;
using Qct.Objects.Entities;

namespace Qct.Marketings
{
    /// <summary>
    /// 默认促销服务（注：二次开发从该促销服务继承）
    /// </summary>
    public class DefaultMarketingService : IMarketingService
    {
        protected IMarketingRuleRepository repository;
        public DefaultMarketingService()
        {
            repository = new MarketingRuleRepository();
        }
        /// <summary>
        ///  匹配促销
        /// </summary>
        /// <param name="order">购物车或者订单</param>
        /// <param name="marketingStage">促销执行的阶段</param>
        /// <returns>促销结果</returns>
        public IEnumerable<IOrderMarketingResult> MatchOrder(IOrder order, OrderMarketingStage marketingStage)
        {
            List<IOrderMarketingResult> contexts = new List<IOrderMarketingResult>();
            switch (marketingStage)
            {
                case OrderMarketingStage.AddProductInShoppingcart:
                    var specialPriceContexts = DoSpecialPrice(order.Cashier.CompanyId, order, repository);
                    var discountContexts = DoDiscount(order.Cashier.CompanyId, order, repository);
                    contexts.AddRange(specialPriceContexts);
                    contexts.AddRange(discountContexts);
                    break;
                case OrderMarketingStage.BeforeCommitOrder:
                    var giftTask = Task.Factory.StartNew(() => { return DoGift(order.Cashier.CompanyId, order, repository); });
                    var integralTask = Task.Factory.StartNew(() => { return DoIntegral(order.Cashier.CompanyId, order, repository); });
                    var tradeInTask = Task.Factory.StartNew(() => { return DoTradeIn(order.Cashier.CompanyId, order, repository); });
                    var couponTask = Task.Factory.StartNew(() => { return DoCoupon(order.Cashier.CompanyId, order, repository); });
                    var reducedCashTask = Task.Factory.StartNew(() => { return DoReducedCash(order.Cashier.CompanyId, order, repository); });
                    Task<IEnumerable<IOrderMarketingResult>>[] tasks = new Task<IEnumerable<IOrderMarketingResult>>[] { giftTask, integralTask, tradeInTask, couponTask, reducedCashTask };
                    Task.WaitAll(tasks);
                    foreach (var task in tasks)
                    {
                        if (!task.IsFaulted && task.IsCompleted)
                        {
                            contexts.AddRange(task.Result);
                        }
                    }
                    break;
            }
            return contexts;
        }
        /// <summary>
        /// 根据促销规则执行促销
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IOrderMarketingResult> ExucuteMarketingRules(int companyId, IEnumerable<MarketingRule> rules)
        {
            List<IOrderMarketingResult> results = new List<IOrderMarketingResult>();
            foreach (var item in rules)
            {
                var builder = (IOrderMarketingResultBuilder)MarketingResultBuilderFactory.Create(companyId, item);
                MarketingResultDirector director = new MarketingResultDirector();
                director.Construct(builder);
                var result = builder.GetResult();
                if (result != null)
                    results.Add(result);
            }
            return results;
        }
        /// <summary>
        /// 执行赠品促销
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="order"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IOrderMarketingResult> DoGift(int companyId, IOrder order, IMarketingRuleRepository repository)
        {
            var rules = repository.GetGiftMarketingRules();
            return ExucuteMarketingRules(companyId, rules);
        }
        /// <summary>
        /// 执行减现促销
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="order"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IOrderMarketingResult> DoReducedCash(int companyId, IOrder order, IMarketingRuleRepository repository)
        {
            var rules = repository.GetReducedCashMarketingRules();
            return ExucuteMarketingRules(companyId, rules);
        }
        /// <summary>
        /// 执行积分促销
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="order"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IOrderMarketingResult> DoIntegral(int companyId, IOrder order, IMarketingRuleRepository repository)
        {
            var rules = repository.GetIntegralMarketingRules();
            return ExucuteMarketingRules(companyId, rules);
        }
        /// <summary>
        /// 执行代金券促销
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="order"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IOrderMarketingResult> DoCoupon(int companyId, IOrder order, IMarketingRuleRepository repository)
        {
            var rules = repository.GetCouponMarketingRules();
            return ExucuteMarketingRules(companyId, rules);
        }
        /// <summary>
        /// 执行换购促销
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="order"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IOrderMarketingResult> DoTradeIn(int companyId, IOrder order, IMarketingRuleRepository repository)
        {
            var rules = repository.GetTradeInMarketingRules();
            return ExucuteMarketingRules(companyId, rules);
        }
        /// <summary>
        /// 执行折扣促销
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="order"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IOrderMarketingResult> DoDiscount(int companyId, IOrder order, IMarketingRuleRepository repository)
        {
            var rules = repository.GetDiscountMarketingRules();
            return ExucuteMarketingRules(companyId, rules);
        }
        /// <summary>
        /// 执行特价促销
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="order"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IOrderMarketingResult> DoSpecialPrice(int companyId, IOrder order, IMarketingRuleRepository repository)
        {
            var rules = repository.GetSpecialPriceMarketingRules();
            return ExucuteMarketingRules(companyId, rules);
        }

        public IEnumerable<IMemberPrepaidOrderMarketingResult> MatchMemberPrepaid(IPrepaidOrder order)
        {
            throw new NotImplementedException();
        }
    }
}