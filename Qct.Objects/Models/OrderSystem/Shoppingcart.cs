using Qct.Domain.CommonObject;
using Qct.Domain.CommonObject.User;
using Qct.Infrastructure.MessageClient;
using Qct.OrderSystem.Exceptions;
using Qct.OrderSystem.Marketings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qct.OrderSystem
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class Shoppingcart : IOrder
    {
        public Shoppingcart(UserCredentials cashier)
        {
            if (cashier == null || !cashier.IsLogin)
                throw new OrderException("非法操作，请先登录系统！");
            ShoppingcartId = Guid.NewGuid();
            CreateDate = DateTime.Now;
            Items = new List<ShoppingcartItem>();
            Cashier = cashier;
            IsSuspended = false;
        }
        /// <summary>
        /// 会员ID(未设置表示普通购物者)
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 购物车编号
        /// </summary>
        public Guid ShoppingcartId { get; set; }
        /// <summary>
        /// 创建购物车的时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 购物车商品列表
        /// </summary>
        public virtual List<ShoppingcartItem> Items { get; set; }
        /// <summary>
        /// 购物车商品列表（用于促销运算遍历）
        /// </summary>
        public IEnumerable<IOrderItem> MarketingOrderItems
        {
            get
            {
                return Items;
            }
        }

        /// <summary>
        /// 收银员
        /// </summary>
        public UserCredentials Cashier { get; set; }
        /// <summary>
        /// 导购员
        /// </summary>
        public UserCredentials Guider { get; set; }
        /// <summary>
        /// 购物车是否被挂起
        /// </summary>
        public bool IsSuspended { get; set; }
        /// <summary>
        /// 新增一项商品
        /// </summary>
        /// <param name="product"></param>
        /// <param name="saleState"></param>
        /// <param name="num"></param>
        public void AddItem(Product product, bool isGift, decimal num)
        {
            if (num == 0m) throw new OrderException("购买数量不能为0！");

            var saleState = SaleState.Normal;
            var price = product.SystemPrice;
            var editedPrice = false;
            if (isGift)//处理手动录入的赠品
            {
                saleState = SaleState.Gift;
                editedPrice = true;
                price = 0m;
            }
            Items.Add(
                new ShoppingcartItem()
                {
                    Id = Guid.NewGuid(),
                    Number = new ProductNumber(num, product.Unit, product.IsWeightProduct()),
                    Product = product,
                    SaleState = saleState,
                    ManualPrice = price,
                    MarketingPrice = price,
                    EditedPrice = editedPrice
                });
        }
        /// <summary>
        /// 移出一项商品
        /// </summary>
        /// <param name="id"></param>
        public void RemoveItem(Guid id)
        {
            var item = Items.FirstOrDefault(o => o.Id == id);
            if (item == null)
                throw new OrderException("指定商品不存在，不能删除！");
            Items.Remove(item);
        }
        /// <summary>
        /// 修改一项商品的数量或者价格
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        /// <param name="num"></param>
        public void EditItem(Guid id, decimal price, decimal num)
        {
            var item = Items.FirstOrDefault(o => o.Id == id);
            EditProductNumber(item, num);
            EditPrice(item, price);
        }
        /// <summary>
        /// 修改商品数量
        /// </summary>
        /// <param name="item"></param>
        /// <param name="num"></param>

        public void EditProductNumber(ShoppingcartItem item, decimal num)
        {
            if (item == null)
            {
                throw new OrderException("指定商品不存在，不能修改数量！");
            }
            if (item.Product.IsWeightProduct())
            {
                throw new OrderException("称重商品不允许修改重量！");
            }
            if (!item.Product.EnableEditNum)
            {
                throw new OrderException("后台设定该商品不允许修改数量！");
            }
            item.Number = new ProductNumber(num, item.Product.Unit, item.Product.IsWeightProduct());

        }
        /// <summary>
        /// 修改商品价格
        /// </summary>
        /// <param name="item"></param>
        /// <param name="price"></param>
        public void EditPrice(ShoppingcartItem item, decimal price)
        {
            if (item == null)
            {
                throw new OrderException("指定商品不存在，不能改价！");
            }
            if (item.Product.IsWeightProduct())
            {
                throw new OrderException("称重商品不允许修改价格！");
            }
            if (!item.Product.EnableEditPrice)
            {
                throw new OrderException("后台设定该商品不允许改价！");
            }
            if (Math.Abs(item.MarketingPrice - price) > 0.005m && item.Product.SystemPrice != price)
            {
                item.ManualPrice = price;
                if (item.SaleState == SaleState.Gift && 0m < price)//赠品改价，如果price>0则状态自动转换为正常销售
                {
                    item.SaleState = SaleState.Normal;
                }
                else if (item.SaleState != SaleState.Gift && price == 0m)//正常商品改价，如果price==0则状态自动转换为销售赠送
                {
                    item.SaleState = SaleState.Gift;
                }
                item.EditedPrice = true;
            }
        }

        /// <summary>
        /// 购物车项目合并查询(货号相同、不是称重商品、未被改价（包含赠品）)
        /// </summary>
        /// <param name="productCode">货号</param>
        /// <param name="isGift">是否为赠品</param>
        /// <returns>返回购物车项目</returns>
        public ShoppingcartItem FindSameOne(string productCode, bool isGift)
        {
            return Items.FirstOrDefault(o =>
            o.Product.EqualsByProductCode(productCode)
            && !o.Product.IsWeightProduct()
            && (!o.EditedPrice || (o.SaleState == SaleState.Gift && isGift)));
        }
        /// <summary>
        /// 设置客户
        /// </summary>
        /// <param name="customerId">客户编号（即会员唯一标识）</param>
        public void SetCustomer(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                throw new OrderException("设置会员失败，会员编号不能为空！");
            CustomerId = customerId;
        }
        /// <summary>
        /// 重置客户
        /// </summary>
        public void ResetCustomer()
        {
            CustomerId = null;
        }
        /// <summary>
        /// 提交订单
        /// </summary>
        /// <returns></returns>
        public Order SubmitOrder(IEnumerable<IOrderMarketingResult> marketingResults)
        {
            var order = new Order(this, marketingResults);
            var publisher = PublisherFactory.Create();
            publisher.PublishAsync(string.Format("PharosRetailing.POSOrderCommited.{0}.{1}.{2}", Cashier.CompanyId, Cashier.StoreId, Cashier.MachineSn), order, true);
            return order;
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
