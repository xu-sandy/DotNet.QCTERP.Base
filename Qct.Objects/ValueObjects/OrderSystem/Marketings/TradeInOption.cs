using System.Collections.Generic;

namespace Qct.Marketings
{
    public class TradeInOption
    {
        public IEnumerable<TradeInItem> TradeInItems { get; set; }
        public string Describe { get; set; }
    }
}
