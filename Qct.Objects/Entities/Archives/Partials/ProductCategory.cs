using Qct.Infrastructure.Data.EntityInterface;
using System;

namespace Qct.Objects.Entities
{
    /// <summary>
    /// 商品分类
    /// </summary>
    public partial class ProductCategory : CompanyEntity
    {
        /// <summary>
        /// 父分类
        /// </summary>
        public virtual ProductCategory Parent { get; set; }
        /// <summary>
        /// 获取从顶级分类到指定分类的路径信息
        /// </summary>
        /// <param name="categorySN">指定分类编号</param>
        /// <returns>从顶级分类到指定分类的路径信息</returns>
        public string GetProductCategoryPath()
        {
            Func<ProductCategory, string> formatFunc = null;
            formatFunc = (ProductCategory productCategory) =>
            {
                if (productCategory.Parent == null)
                {
                    return productCategory.CategorySN.ToString();
                }
                else
                {
                    return (formatFunc(productCategory.Parent) + "/" + productCategory.CategorySN);
                }
            };
            return formatFunc(this);
        }
        /// <summary>
        /// 获取从顶级分类到指定分类的路径下的标题信息
        /// </summary>
        /// <param name="categorySN"></param>
        /// <returns>从顶级分类到指定分类的路径下的标题信息</returns>
        public string GetProductCategoryTitlePath()
        {
            Func<ProductCategory, string> formatFunc = null;
            formatFunc = (ProductCategory productCategory) =>
            {
                if (productCategory.Parent == null)
                {
                    return productCategory.Title;
                }
                else
                {
                    return (formatFunc(productCategory.Parent) + "/" + productCategory.Title);
                }
            };
            return formatFunc(this);
        }
        /// <summary>
        /// 获取分类节点信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetProductCategoryPath();
        }
    }
}
