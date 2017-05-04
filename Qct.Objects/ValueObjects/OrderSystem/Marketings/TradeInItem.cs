using Qct.Objects.Entities;

namespace Qct.Marketings
{
    public class TradeInItem
    {
        /// <summary>
        /// 商品信息
        /// </summary>
        public ProductRecord Product { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public ProductNumber Number { get; set; }
        /// <summary>
        /// 特价金额
        /// </summary>
        public decimal Amount { get; set; }

    }
}
