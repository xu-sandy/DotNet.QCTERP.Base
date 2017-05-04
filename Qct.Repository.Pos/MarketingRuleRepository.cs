using Qct.Marketings;
using System;
using System.Linq;
using System.Collections.Generic;
using Qct.IRepository;

namespace Qct.Repository
{
    public class MarketingRuleRepository : IMarketingRuleRepository
    {
        public IEnumerable<MarketingRule> GetCouponMarketingRules()
        {
            throw new NotImplementedException();
        }

        public IQueryable<MarketingRule> GetCurrentMarketingRules()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MarketingRule> GetDiscountMarketingRules()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MarketingRule> GetGiftMarketingRules()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MarketingRule> GetIntegralMarketingRules()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MarketingRule> GetReducedCashMarketingRules()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MarketingRule> GetSpecialPriceMarketingRules()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MarketingRule> GetTradeInMarketingRules()
        {
            throw new NotImplementedException();
        }
    }
}
