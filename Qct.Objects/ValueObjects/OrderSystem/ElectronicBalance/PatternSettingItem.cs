namespace Qct.ElectronicBalance
{
    /// <summary>
    /// 电子称工作模式下设置参数枚举
    /// </summary>
    public enum PatternSettingItem
    {
        /// <summary>
        /// 条码长度
        /// </summary>
        Length,
        /// <summary>
        /// 前缀标识
        /// </summary>
        FlagCode,
        /// <summary>
        /// 货号长度
        /// </summary>
        ProductCodeLength,
        /// <summary>
        /// 条码长度
        /// </summary>
        BarcodeLength,
        /// <summary>
        /// 重量精度
        /// </summary>
        WeightPrecision,
        /// <summary>
        /// 重量
        /// </summary>
        WeightLength,
        /// <summary>
        /// 金额精度
        /// </summary>
        AmountPrecision,
        /// <summary>
        /// 金额长度
        /// </summary>
        AmountLength,
        /// <summary>
        /// 校验码
        /// </summary>
        CheckCode
    }
}
