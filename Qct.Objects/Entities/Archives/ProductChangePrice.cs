// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：linbl
// 创建时间：2017-04-19
// 描述信息：
// --------------------------------------------------

using System;

namespace Qct.Objects.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public partial class ProductChangePrice
	{
		/// <summary>
		/// 
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int CompanyId { get; set; }

		/// <summary>
		/// 变价主表编号
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string ChangePriceSN { get; set; }

		/// <summary>
		/// 调价范围（1-商品档案，2-商品档案及所有门店，3-指定门店）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short RangeType { get; set; }

		/// <summary>
		/// 相关门店Id
		/// [长度：250]
		/// </summary>
		public string StoreIds { get; set; }

		/// <summary>
		/// 生效日期
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public DateTime ValidDate { get; set; }

		/// <summary>
		/// 备注
		/// [长度：200]
		/// </summary>
		public string Note { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }

		/// <summary>
		/// 
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// </summary>
		public string AuditorUID { get; set; }

		/// <summary>
		/// 
		/// [长度：23，小数位数：3]
		/// </summary>
		public DateTime AuditorDT { get; set; }
	}
}
