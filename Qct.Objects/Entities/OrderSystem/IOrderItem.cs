using Qct.Objects.Entities;

namespace Qct.Objects.Entities
{
    public interface IOrderItem
    {
        /// <summary>
        /// 商品信息
        /// </summary>
        ProductRecord Product { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        ProductNumber Number { get; set; }
        /// <summary>
        /// 销售手动改价
        /// </summary>
        decimal ManualPrice { get; set; }
        /// <summary>
        /// 后台促销价
        /// </summary>
        decimal MarketingPrice { get; set; }

        /// <summary>
        /// 商品销售状态
        /// </summary>
        SaleState SaleState { get; set; }
        /// <summary>
        /// 是否已被手动改价
        /// </summary>
        bool EditedPrice { get; set; }
    }

}
