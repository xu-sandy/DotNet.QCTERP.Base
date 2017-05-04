using Qct.Exceptions;
using Qct.Objects.Entities;

namespace Qct
{
    public static class OrderItemExtensions
    {
        /// <summary>
        /// 是否为手动录入赠品
        /// </summary>
        /// <returns></returns>
        public static bool IsGift(this IOrderItem item)
        {
            if (item == null) return false;
            return item.SaleState == SaleState.Gift;
        }

        /// <summary>
        /// 获取商品是否拥有后台促销价，改价商品不参与后台促销
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool HasMarketingPrice(this IOrderItem item)
        {
            if (item == null) throw new OrderException("获取后台促销状态失败，商品不能为空！");
            return !item.EditedPrice && item.MarketingPrice != item.Product.SysPrice;
        }
        /// <summary>
        /// 获取商品小计
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static decimal Subtotal(this IOrderItem item)
        {
            if (item == null) throw new OrderException("获取商品小计失败，商品不能为空！");
            if (item.EditedPrice)
            {
                return item.ManualPrice * item.Number.UnitNumber;
            }
            else if (item.HasMarketingPrice())
            {
                return item.MarketingPrice * item.Number.UnitNumber;
            }
            else
            {
                return item.Product.SysPrice * item.Number.UnitNumber;
            }
        }
        /// <summary>
        /// 获取商品优惠小计
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static decimal ItemDiscount(this IOrderItem item)
        {
            if (item == null) throw new OrderException("获取商品优惠小计失败，商品不能为空！");
            if (item.EditedPrice && item.Product.SysPrice > item.ManualPrice)
            {
                return (item.Product.SysPrice - item.ManualPrice) * item.Number.UnitNumber;
            }
            else if (item.HasMarketingPrice() && item.Product.SysPrice > item.MarketingPrice)
            {
                return (item.Product.SysPrice - item.MarketingPrice) * item.Number.UnitNumber;
            }
            return 0;
        }

        /// <summary>
        /// 获取销售价
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static decimal GetItemPrice(this IOrderItem item)
        {
            if (item == null) throw new OrderException("获取商品销售价失败，商品不能为空！");
            if (item.EditedPrice)
            {
                return item.ManualPrice;
            }
            else if (item.HasMarketingPrice())
            {
                return item.MarketingPrice;
            }
            else
            {
                return item.Product.SysPrice;
            }
        }
    }
}
