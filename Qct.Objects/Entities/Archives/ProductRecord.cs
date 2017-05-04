// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：linbl
// 创建时间：2017-04-18
// 描述信息：
// --------------------------------------------------

using Qct.Objects.ValueObjects;
using System;

namespace Qct.Objects.Entities
{
    /// <summary>
    /// 用于管理本系统的所有产品档案基本信息
    /// </summary>
    public partial class ProductRecord
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 货号 （自动 生成， 全局唯一） 
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 条形码（全局唯一）
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 品名
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 规格
        /// [长度：50]
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 箱规
        /// [长度：50]
        /// </summary>
        public string BoxBoard { get; set; }

        /// <summary>
        /// 品牌SN
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int BrandSN { get; set; }

        /// <summary>
        /// 主供应商ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        /// 产地ID（来自城市ID）
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 品类SN（大类）
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int CategorySN { get; set; }

        /// <summary>
        /// 计量大单位ID（来自数据字典表）
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int BigUnitId { get; set; }

        /// <summary>
        /// 计量小单位ID（来自数据字典表）
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int SubUnitId { get; set; }

        /// <summary>
        /// 进价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal BuyPrice { get; set; }

        /// <summary>
        /// 系统售价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal SysPrice { get; set; }

        /// <summary>
        /// 批发价
        /// [长度：19，小数位数：4]
        /// [默认值：((0))]
        /// </summary>
        public decimal TradePrice { get; set; }

        /// <summary>
        /// 加盟价
        /// [长度：19，小数位数：4]
        /// [默认值：((0))]
        /// </summary>
        public decimal JoinPrice { get; set; }

        /// <summary>
        /// 产品性质（0:单品、1:组合、2:拆分）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public ProductType Nature { get; set; }


        /// <summary>
        /// 前台优惠（1:允许调价、0:不允许调价)
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public bool Favorable { get; set; }

        /// <summary>
        /// 保质期（0:不限）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short Expiry { get; set; }

        /// <summary>
        /// 保质期单位（1:天、2:月、3:年）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public ExpiryUnit ExpiryUnit { get; set; }

        /// <summary>
        /// 生产厂家
        /// [长度：50]
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 计价方式（1:计件、2:称重）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public MeteringMode ValuationType { get; set; }

        /// <summary>
        /// 退货标志（1:允许、0:不允许）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public bool IsReturnSale { get; set; }

        /// <summary>
        /// 订货标志（1:允许、0:不允许）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public bool IsAcceptOrder { get; set; }

        /// <summary>
        /// 物价员（UID）
        /// [长度：40]
        /// [默认值：((-1))]
        /// </summary>
        public string RaterUID { get; set; }

        /// <summary>
        /// 库存预警（下限数量）
        /// [长度：5]
        /// [默认值：((5))]
        /// </summary>
        public short InventoryWarning { get; set; }

        /// <summary>
        /// 保质期预警（天）
        /// [长度：5]
        /// [默认值：((5))]
        /// </summary>
        public short ValidityWarning { get; set; }

        /// <summary>
        /// 产品状态（0:已下架、1:已上架）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public ProductState State { get; set; }

        /// <summary>
        /// 一品多码
        /// [长度：150]
        /// </summary>
        public string Barcodes { get; set; }

        /// <summary>
        /// 进项税率
        /// [长度：19，小数位数：4]
        /// [默认值：((17))]
        /// </summary>
        public decimal StockRate { get; set; }

        /// <summary>
        /// 销售税率
        /// [长度：19，小数位数：4]
        /// [默认值：((17))]
        /// </summary>
        public decimal SaleRate { get; set; }

        /// <summary>
        /// 库存预警（上限数量）
        /// [长度：5]
        /// [默认值：((5))]
        /// </summary>
        public short InventoryWarningMax { get; set; }

        /// <summary>
        /// 建档时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 是否发生业务关系(0:否,1:是)
        /// [长度：1]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public bool IsRelationship { get; set; }
    }
}
