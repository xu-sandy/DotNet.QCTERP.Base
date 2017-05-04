using Qct.Infrastructure.Data.EntityInterface;
using Qct.Objects.ValueObjects;

namespace Qct.Objects.Entities
{
    public partial class ProductRecord : CompanyEntity
    {
        /// <summary>
        /// 分类
        /// </summary>
        public virtual ProductCategory Category { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public virtual ProductBrand Brand { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public virtual SysDataDictionary SubUnit { get; set; }
        /// <summary>
        /// 通过条码判断商品是否为相同商品
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public bool EqualsByBarcode(string barcode)
        {
            barcode = barcode.Trim();
            if (Barcode.Trim() == barcode) { return true; }
            if (("," + Barcodes + ",").Contains("," + barcode + ","))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 通过货号比较是否为本商品
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public bool EqualsByProductCode(string productCode)
        {
            return ProductCode == productCode;
        }
        /// <summary>
        /// 是否为称重商品
        /// </summary>
        /// <returns></returns>
        public bool IsWeightProduct()
        {
            return ValuationType == MeteringMode.Weight;
        }

    }
}
