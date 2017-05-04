using Qct.Infrastructure.Data;
using Qct.Objects.Entities;
using System.Collections.Generic;

namespace Qct.IRepository
{
    /// <summary>
    /// 商品分类仓储接口
    /// </summary>
    public interface IProductCategoryRepository : IEFRepository<ProductCategory>
    {
        /// <summary>
        /// 获取指定分类信息，如未指定返回所有顶级分类
        /// </summary>
        /// <param name="categorySNs"></param>
        /// <returns></returns>
        IEnumerable<ProductCategory> GetProductCategorys(params int[] categorySNs);

        /// <summary>
        /// 获取指定分类，状态为禁用时，返回null
        /// </summary>
        /// <param name="categorySN">指定分类编号</param>
        /// <returns></returns>
        ProductCategory Get(int categorySN);

        /// <summary>
        /// 忽略分类状态获取指定分类（用于历史查询）
        /// </summary>
        /// <param name="categorySN">指定分类编号</param>
        /// <returns></returns>
        ProductCategory GetIngoreState(int categorySN);
        /// <summary>
        /// 获取顶层类别
        /// </summary>
        /// <param name="all">true->取所有，状态不可用时有*标识</param>
        /// <returns></returns>
        IEnumerable<ProductCategory> GetRootCategorys(bool all = false);

    }
}
