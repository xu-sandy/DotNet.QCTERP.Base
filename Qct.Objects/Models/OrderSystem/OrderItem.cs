using Qct.Domain.CommonObject;

namespace Qct.OrderSystem
{
    public class OrderItem : IOrderItem
    {
        /// <summary>
        /// 商品信息
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public ProductNumber Number { get; set; }
        /// <summary>
        /// 销售手动改价
        /// </summary>
        public decimal ManualPrice { get; set; }
        /// <summary>
        /// 后台促销价
        /// </summary>
        public decimal MarketingPrice { get; set; }
        /// <summary>
        /// 商品销售状态
        /// </summary>
        public SaleState SaleState { get; set; }
        /// <summary>
        /// 是否已经被手动改价
        /// </summary>
        public bool EditedPrice { get; set; }
    }
}
