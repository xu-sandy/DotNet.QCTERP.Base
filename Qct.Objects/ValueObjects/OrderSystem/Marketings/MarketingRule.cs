using Qct.Infrastructure.Data.EntityInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Marketings
{
    public class MarketingRule : GuidIdEntity
    {
        public Guid Id { get; set; }
        public int MarketingType { get; internal set; }
    }
}
