using System.Collections.Generic;
using System.Linq;

namespace Qct.Domain.CommonObject
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 主条码
        /// </summary>
        public string MainBarcode { get; set; }
        /// <summary>
        /// 一品多码
        /// </summary>
        public IEnumerable<string> OneProductMultipleBarcodes { get; set; }
        /// <summary>
        /// 货号
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 系统售价
        /// </summary>
        public decimal SystemPrice { get; set; }

        /// <summary>
        /// 进价
        /// </summary>
        public decimal BuyPrice { get; set; }
        /// <summary>
        /// 是否允许修改销售数量
        /// </summary>
        public bool EnableEditNum { get; set; }
        /// <summary>
        /// 是否允许修改售价
        /// </summary>
        public bool EnableEditPrice { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public Brand Brand { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public CategoryNode Category { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public ProductUnit Unit { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductType ProductType { get; set; }
        /// <summary>
        /// 通过条码比较是否为本商品
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public bool EqualsByBarcode(string barcode)
        {
            return OneProductMultipleBarcodes.Any(o => o == barcode) || MainBarcode == barcode;
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
            return ProductType == ProductType.Weigh;
        }

    }
}
