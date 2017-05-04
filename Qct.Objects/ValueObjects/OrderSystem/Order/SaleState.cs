using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.OrderSystem
{
    /// <summary>
    /// 商品销售状态
    /// </summary>
    public enum SaleState
    {
        /// <summary>
        /// 正常销售商品
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 赠品
        /// </summary>
        Gift = 1,

        /*以下三种状态不会在购物车状态中出现，只在订单生成后出现*/
        /// <summary>
        /// 后台促销赠品
        /// </summary>
        MarketingGift = 2,
        /// <summary>
        /// 换购商品
        /// </summary>
        TradeInProduct = 3,
        /// <summary>
        /// 捆绑商品
        /// </summary>
        Binding = 3
    }
}
