using Qct.Domain.CommonObject;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using Qct.Marketings;
using System;

namespace Qct.Objects.Entities
{
    /// <summary>
    /// 购物车明细
    /// </summary>
    public partial class ShoppingcartItem : IOrderItem
    {
        ProductRecord product;
        /// <summary>
        /// 用于删除、修改购物车的编辑Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 商品信息
        /// </summary>
        public ProductRecord Product
        {
            get { return product; }
            set
            {
                product = value;
                EnableEditPrice = product.Favorable;
                EnableEditNum = product.ValuationType == MeteringMode.Weight;
            }
        }
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
        /// <summary>
        /// 是否允许改数量
        /// </summary>
        public bool EnableEditNum { get; set; }
        /// <summary>
        /// 是否允许改价格
        /// </summary>
        public bool EnableEditPrice { get; set; }
    }
}