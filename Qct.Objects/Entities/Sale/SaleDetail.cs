// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：
// 创建时间：2017-05-03
// 描述信息：
// --------------------------------------------------

using System;

namespace Qct.Objects.Entities
{
	/// <summary>
	/// 用于管理本系统的所有商品销售明细信息（主表：SaleOrders）
	/// </summary>
	[Serializable]
	public class SaleDetail
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
		/// 
		/// [长度：20]
		/// </summary>
		public string ProductCode { get; set; }

		/// <summary>
		/// 扫入条码
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string ScanBarcode { get; set; }

		/// <summary>
		/// 流水号
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string PaySN { get; set; }

		/// <summary>
		/// 商品条码
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string Barcode { get; set; }

		/// <summary>
		/// 品名
		/// [长度：50]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 购买数量
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal PurchaseNumber { get; set; }

		/// <summary>
		/// 
		/// [长度：18，小数位数：8]
		/// </summary>
		public decimal AveragePrice { get; set; }

		/// <summary>
		/// 系统进价
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
		/// 交易价
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal ActualPrice { get; set; }

		/// <summary>
		/// 
		/// [长度：19，小数位数：4]
		/// </summary>
		public decimal Total { get; set; }

		/// <summary>
		/// 销售分类ID（来自数据字典）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public int SalesClassifyId { get; set; }

		/// <summary>
		/// 备注
		/// [长度：200]
		/// </summary>
		public string Memo { get; set; }

		
	}
}
