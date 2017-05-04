using Qct.Objects.Entities;

namespace Qct
{
    /// <summary>
    /// 商品数量
    /// </summary>
    public class ProductNumber
    {
        /// <summary>
        /// 商品数量及单位信息
        /// </summary>
        /// <param name="number"></param>
        /// <param name="unit"></param>
        /// <param name="isWeightUnit"></param>
        public ProductNumber(decimal number, SysDataDictionary unit, bool isWeightUnit)
        {
            UnitNumber = number;
            Unit = unit;
            IsWeight = isWeightUnit;
            if (isWeightUnit)
                CountNumber = 1;
            else
                CountNumber = number;
        }
        /// <summary>
        /// 商品单位数量
        /// </summary>
        public decimal UnitNumber { get; set; }
        /// <summary>
        /// 商品单位
        /// </summary>
        public SysDataDictionary Unit { get; set; }
        /// <summary>
        /// 计件数量
        /// </summary>
        public decimal CountNumber { get; set; }
        /// <summary>
        /// 是否为重量单位
        /// </summary>
        public bool IsWeight { get; set; }
        /// <summary>
        /// 格式化成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0:###.###} {1}", UnitNumber, Unit);
        }
    }
}
