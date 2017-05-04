using System;
using System.Linq;
using System.Collections.Generic;
using Qct.Exceptions;
using Qct.Domain.CommonObject.User;
using Qct.IRepository;
using Qct;
using Qct.Objects.Entities;

namespace Qct.Repository.Pos
{
    /// <summary>
    /// 购物车资源库，在同一时间内，一位收银员只允许一个购物车处于工作状态
    /// </summary>
    public class ShoppingcartRepository : IShoppingcartRepository
    {
        /// <summary>
        /// 在设备上运行时，不需要持久化。后台运行时，web是无状态的需要持久化
        /// </summary>
        private readonly static List<Shoppingcart> shoppingcarts = new List<Shoppingcart>();

        /// <summary>
        /// 从资源库删除一个购物车
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notFindThowException"></param>
        public void Delete(Guid id, bool notFindThowException = false)
        {
            var shoppingcart = shoppingcarts.FirstOrDefault(o => o.ShoppingcartId == id);
            if (notFindThowException && shoppingcart == null)
            {
                throw new OrderException("未找到指定的购物车！");
            }
            else
            {
                shoppingcarts.Remove(shoppingcart);
            }
        }
        /// <summary>
        /// 从资源库获取当前购物车，如果未找到则创建新的
        /// </summary>
        /// <param name="cashier"></param>
        /// <returns></returns>
        public Shoppingcart GetCurrentShoppingcartOrCreate(UserCredentials cashier)
        {
            var shoppingcart = shoppingcarts.FirstOrDefault(o => !o.IsSuspended);
            if (shoppingcart == null)
            {
                shoppingcart = new Shoppingcart(cashier);
                shoppingcarts.Add(shoppingcart);
            }
            return shoppingcart;
        }
        /// <summary>
        /// 通过购物车编号获取购物车
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Shoppingcart Get(Guid id)
        {
            return shoppingcarts.FirstOrDefault(o => o.ShoppingcartId == id);
        }

        /// <summary>
        /// 获取挂单列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Shoppingcart> GetSuspendedList()
        {
            return shoppingcarts.Where(o => o.IsSuspended).ToList();
        }
        /// <summary>
        ///设置挂起购物车重新激活，如果设置激活时，当前购物车存在子项，将自动挂起，不存在则删除该购物车
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Shoppingcart Resume(Guid id)
        {
            var current = shoppingcarts.FirstOrDefault(o => !o.IsSuspended);
            if (current != null)
            {
                if (current.Items.Any())
                {
                    current.IsSuspended = true;
                }
                else
                {
                    shoppingcarts.Remove(current);
                }
            }
            current = Get(id);
            current.IsSuspended = false;
            return current;
        }
    }
}
