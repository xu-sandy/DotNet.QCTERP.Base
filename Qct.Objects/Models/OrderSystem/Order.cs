using Qct.Domain.CommonObject.User;
using Qct.Infrastructure.MessageClient;
using Qct.OrderSystem.Exceptions;
using Qct.OrderSystem.Marketings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Qct.OrderSystem
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order : IOrder
    {
        private Shoppingcart shoppingcart;
        private IEnumerable<IOrderMarketingResult> marketingResults;
        /// <summary>
        /// 初始订单
        /// </summary>
        /// <param name="shoppingcart"></param>
        /// <param name="marketingContexts"></param>
        public Order(Shoppingcart shoppingcart, IEnumerable<IOrderMarketingResult> marketingResults)
        {
            if (shoppingcart == null)
                throw new OrderException("提交订单时，订单不能为空！");
            if (marketingResults == null)
                throw new OrderException("提交订单前，必须先运行促销！");

            this.marketingResults = marketingResults;

            CreateDate = DateTime.Now;
            Items = new List<OrderItem>();
            this.shoppingcart = shoppingcart;
            CustomerId = shoppingcart.CustomerId;
            CreateDate = DateTime.Now;


            foreach (var item in shoppingcart.Items)
            {
                Items.Add(new OrderItem()
                {
                    MarketingPrice = item.MarketingPrice,
                    Number = item.Number,
                    Product = item.Product,
                    ManualPrice = item.ManualPrice,
                    SaleState = item.SaleState,
                    EditedPrice = item.EditedPrice

                });
            }
            foreach (var item in marketingResults)
            {
                var items = item.GetGiftsOrTradeInItems();
                if (items != null && items.Any())
                    Items.AddRange(items);
            }
            Cashier = shoppingcart.Cashier;
            Guider = shoppingcart.Guider;
            OrderId = new OrderSn(Cashier.CompanyId, Cashier.StoreId, Cashier.MachineSn);

            OrderState = OrderState.Created;
        }
        /// <summary>
        /// 订单Id(即流水号)
        /// </summary>
        public OrderSn OrderId { get; set; }

        /// <summary>
        /// 会员ID(未设置表示普通购物者)
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 订单项
        /// </summary>
        public List<OrderItem> Items { get; set; }
        /// <summary>
        /// 订单项目（用于促销运算遍历）
        /// </summary>
        public IEnumerable<IOrderItem> MarketingOrderItems
        {
            get
            {
                return Items;
            }
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderState OrderState { get; set; }

        /// <summary>
        /// 收银员
        /// </summary>
        public UserCredentials Cashier { get; set; }
        /// <summary>
        /// 导购员
        /// </summary>
        public UserCredentials Guider { get; set; }
        /// <summary>
        /// 订单支付明细
        /// </summary>
        public IEnumerable<OrderPay> OrderPays { get; set; }
        /// <summary>
        /// 设置支付完成
        /// </summary>
        /// <param name="orderPays"></param>
        public void SetOrderPayComplete(IEnumerable<OrderPay> orderPays)
        {
            if (orderPays == null)
                throw new OrderException("支付信息为空，设置失败！");
            OrderPays = orderPays;
            OrderState = OrderState.Paid;
            var publisher = PublisherFactory.Create();
            publisher.PublishAsync(string.Format("Qct.LocalPos.OrderPaid.{0}.{1}.{2}", Cashier.CompanyId, Cashier.StoreId, Cashier.MachineSn), this, true);
        }

        /// <summary>
        /// 设置订单完成
        /// </summary>
        public void SetOrderComplete()
        {
            //必须先锁定流水号
            OrderId.SetLockIncreasingNumber();

            var publisher = PublisherFactory.Create();
            publisher.PublishAsync(string.Format("Qct.LocalPos.OrderCompleted.{0}.{1}.{2}", Cashier.CompanyId, Cashier.StoreId, Cashier.MachineSn), this, true);
        }
        /// <summary>
        /// 获取商品件数，称重商品一称算一件
        /// </summary>
        /// <returns></returns>
        public decimal GetProductCount()
        {
            return Items.Sum(o => o.Number.CountNumber);
        }

        /// <summary>
        /// 获取购物车总计金额
        /// </summary>
        /// <returns></returns>
        public decimal GetTotal()
        {
            var total = Items.Sum(o => o.Subtotal());
            return total;
        }
        /// <summary>
        /// 获取购物车已优惠金额【只记优惠部分，不计多收部分】
        /// </summary>
        /// <returns></returns>
        public decimal GetDiscount()
        {
            var discount = Items.Sum(o => o.ItemDiscount());
            return discount;
        }
    }
}
