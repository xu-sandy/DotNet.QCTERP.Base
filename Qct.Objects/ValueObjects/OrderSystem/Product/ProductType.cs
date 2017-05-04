namespace Qct.Domain.CommonObject
{
    /// <summary>
    /// 商品类型
    /// </summary>
    public enum ProductType
    {
        /// <summary>
        /// 正常商品
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 称重商品
        /// </summary>
        Weigh = 1,

        /// <summary>
        /// 捆绑销售商品
        /// </summary>
        Bundling = 2,

        /// <summary>
        /// 拆分商品
        /// </summary>
        Split = 3,

        /// <summary>
        /// 组合商品
        /// </summary>
        Combination = 4

    }
}
