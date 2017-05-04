using Qct.Domain.CommonObject.User;
using System.Collections.Generic;

namespace Qct.OrderSystem
{
    public interface IOrder
    {
        /// <summary>
        /// 会员ID(未设置表示普通购物者)
        /// </summary>
        string CustomerId { get; set; }
        /// <summary>
        /// 收银员
        /// </summary>
        UserCredentials Cashier { get; set; }
        /// <summary>
        /// 导购员
        /// </summary>
        UserCredentials Guider { get; set; }

        /// <summary>
        /// 促销商品范围
        /// </summary>
        IEnumerable<IOrderItem> MarketingOrderItems { get; }
    }

}