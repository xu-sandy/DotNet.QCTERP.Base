using Qct.IServices;
using Qct.Marketings;

namespace Qct.Services.Builders
{
    public class MarketingResultBuilderFactory
    {
        public static IMarketingResultBuilder Create(int companyId, MarketingRule rule)
        {
            switch (companyId)
            {
                //公司定制实现调用入口
                /*
                 case 108:
                 switch (rule.MarketingType)
                 { 
                     case SpecialPrice:
                         return new C108SpecialPriceMarketingResultBuilder(rule);
                     default:
                         goto DefaultRule;
                 }
                 */
                default:
                    DefaultRule:
                    {
                        switch (rule.MarketingType)
                        {
                            //To do case 
                            default:
                                return null;
                        }
                    }
            }
        }
    }
}
