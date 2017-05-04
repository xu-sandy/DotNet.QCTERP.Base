using Qct.IServices;
using Qct.Marketings;

namespace Qct.Services
{
    public static class MarketingServiceFactory
    {
        public static IMarketingService Create(int companyId)
        {
            switch (companyId)
            {
                //case 调用二次开发MarketingService
                default:
                    return new DefaultMarketingService();
            }
        }
    }
}
