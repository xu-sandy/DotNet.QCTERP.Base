using Qct.Domain.CommonObject.User;
using System;
using System.Collections.Generic;
using Qct.Objects.Entities;

namespace Qct.IRepository
{
    /// <summary>
    /// 购物车仓储接口（可对购物车进行持久化或者缓存）
    /// </summary>
    public interface IShoppingcartRepository
    {
        /// <summary>
        /// 获取当前购物车，不存在则创建
        /// </summary>
        /// <param name="cashier"></param>
        /// <returns></returns>
        Shoppingcart GetCurrentShoppingcartOrCreate(UserCredentials cashier);
        /// <summary>
        /// 获取挂起购物车列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Shoppingcart> GetSuspendedList();
        /// <summary>
        /// 从资源库删除一个购物车
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notFindThowException"></param>
        void Delete(Guid id, bool notFindThowException = false);
        /// <summary>
        ///设置挂起购物车，重新激活。如果设置激活时，当前购物车存在子项，将自动挂起，不存在则删除该购物车
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Shoppingcart Resume(Guid id);
        /// <summary>
        /// 通过购物车编号获取购物车
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Shoppingcart Get(Guid id);
    }
}
