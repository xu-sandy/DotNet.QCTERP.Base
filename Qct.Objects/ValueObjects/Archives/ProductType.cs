namespace Qct.Objects.ValueObjects
{
    /// <summary>
    /// 商品类型
    /// </summary>
    public enum ProductType : short
    {
        /// <summary>
        /// 正常商品
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 组合商品
        /// </summary>
        Combination = 1,
        /// <summary>
        /// 拆分商品
        /// </summary>
        Split = 2
    }
}
