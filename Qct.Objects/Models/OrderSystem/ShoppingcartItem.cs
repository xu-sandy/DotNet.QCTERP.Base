using Qct.Domain.CommonObject;
using Qct.OrderSystem.Marketings;
using System;

namespace Qct.OrderSystem
{
    /// <summary>
    /// 购物车明细
    /// </summary>
    public partial class ShoppingcartItem : IOrderItem
    {
        /// <summary>
        /// 用于删除、修改购物车的编辑Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 商品信息
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public ProductNumber Number { get; set; }
        /// <summary>
        /// 销售改价
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
        /// 是否已被手动改价
        /// </summary>
        public bool EditedPrice { get; set; }

    }
}