using Qct.IServices;

namespace Qct.Services.Builders
{
    public class MarketingResultDirector
    {
        public void Construct(IMarketingResultBuilder builder)
        {
            builder.BuilderMarketingContext();
            builder.BuilderMarketingResult();
        }
    }
}
