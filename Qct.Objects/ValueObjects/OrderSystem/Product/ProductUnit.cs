using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Domain.CommonObject
{
    /// <summary>
    /// 商品计量单位
    /// </summary>
    public class ProductUnit
    {
        /// <summary>
        /// 计量单位初始化
        /// </summary>
        /// <param name="unitName"></param>
        /// <param name="unitValue"></param>
        public ProductUnit(string unitName, int unitValue) { }
        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 单位对应的字典值
        /// </summary>
        public int UnitValue { get; set; }
        /// <summary>
        /// 重载字符串格式化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return UnitName;
        }
        /// <summary>
        /// 重载对象比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is int)
            {
                return UnitValue.Equals(obj);
            }
            else if (obj is ProductUnit)
            {
                var unit = obj as ProductUnit;
                return UnitValue.Equals(unit.UnitValue);
            }
            return false;
        }
        /// <summary>
        /// 重载返回的哈希代码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return UnitValue.GetHashCode();
        }
    }
}
