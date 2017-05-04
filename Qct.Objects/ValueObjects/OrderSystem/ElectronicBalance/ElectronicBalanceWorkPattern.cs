namespace Qct.ElectronicBalance
{
    /// <summary>
    /// 电子称工作模式【条码生成模式】
    /// </summary>
    public enum ElectronicBalanceWorkPattern
    {
        /// <summary>
        /// 13位称重条码（标识符+货号+重量+校验码）
        /// </summary>
        Length13_FlagCode_ProductCode_Weight_CheckCode = 1,
        /// <summary>
        /// 13位称重条码（标识符+货号+重量）
        /// </summary>
        Length13_FlagCode_ProductCode_Weight = 2,
        /// <summary>
        /// 13位称重条码（标识符+条码+重量+校验码）
        /// </summary>
        Length13_FlagCode_Barcode_Weight_CheckCode = 3,
        /// <summary>
        /// 13位称重条码（标识符+条码+重量+校验码）
        /// </summary>
        Length13_FlagCode_Barcode_Weight = 4,
        /// <summary>
        /// 13位称重条码（标识符+货号+金额+校验码）
        /// </summary>
        Length13_FlagCode_ProductCode_Amount_CheckCode = 5,
        /// <summary>
        /// 13位称重条码（标识符+货号+金额）
        /// </summary>
        Length13_FlagCode_ProductCode_Amount = 6,
        /// <summary>
        /// 13位称重条码（标识符+条码+金额+校验码）
        /// </summary>
        Length13_FlagCode_Barcode_Amount_CheckCode = 7,
        /// <summary>
        /// 13位称重条码（标识符+条码+金额）
        /// </summary>
        Length13_FlagCode_Barcode_Amount = 8,
        /// <summary>
        /// 18位称重条码（标识符+货号+重量+金额+校验码）
        /// </summary>
        Length18_FlagCode_ProductCode_Weight_Amount_CheckCode = 9,
        /// <summary>
        /// 18位称重条码（标识符+货号+重量+金额）
        /// </summary>
        Length18_FlagCode_ProductCode_Weight_Amount = 10,
        /// <summary>
        /// 18位称重条码（标识符+条码+重量+金额+校验码）
        /// </summary>
        Length18_FlagCode_Barcode_Weight_Amount_CheckCode = 11,
        /// <summary>
        /// 18位称重条码（标识符+条码+重量+金额）
        /// </summary>
        Length18_FlagCode_Barcode_Weight_Amount = 12,
        /// <summary>
        /// 18位称重条码（标识符+货号+金额+重量+校验码）
        /// </summary>
        Length18_FlagCode_ProductCode_Amount_Weight_CheckCode = 13,
        /// <summary>
        /// 18位称重条码（标识符+货号+金额+重量）
        /// </summary>
        Length18_FlagCode_ProductCode_Amount_Weight = 14,
        /// <summary>
        /// 18位称重条码（标识符+条码+金额+重量+校验码）
        /// </summary>
        Length18_FlagCode_Barcode_Amount_Weight_CheckCode = 15,
        /// <summary>
        /// 18位称重条码（标识符+条码+金额+重量）
        /// </summary>
        Length18_FlagCode_Barcode_Amount_Weight = 16,
    }
    /// <summary>
    /// 电子秤工作模式
    /// </summary>
    public static class ElectronicBalanceWorkPatternExtension
    {
        /// <summary>
        /// 已知类型返回对应的条码长度，否则返回0
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static int GetLength(this ElectronicBalanceWorkPattern pattern)
        {
            switch (pattern)
            {
                case ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Weight_CheckCode:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Weight:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_Barcode_Weight_CheckCode:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_Barcode_Weight:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Amount_CheckCode:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Amount:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_Barcode_Amount_CheckCode:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_Barcode_Amount:
                    return 13;
                case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Amount_Weight:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Amount_Weight_CheckCode:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Weight_Amount:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Weight_Amount_CheckCode:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Amount_Weight:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Amount_Weight_CheckCode:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Weight_Amount:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Weight_Amount_CheckCode:
                    return 18;
                default:
                    return 0;
            }
        }
    }
}
