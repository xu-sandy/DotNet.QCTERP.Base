// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：linbl
// 创建时间：2017-04-18
// 描述信息：
// --------------------------------------------------

using System;

namespace Qct.Objects.Entities.Archives
{
    /// <summary>
    /// 商品组合拆分记录项
    /// </summary>
    public partial class ProductGroupOrSplit
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
		/// 子项货号
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string ChildProductCode { get; set; }

		/// <summary>
		/// 
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string ParentProductCode { get; set; }

		/// <summary>
		/// 
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short Nature { get; set; }

		/// <summary>
		/// 
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
		public decimal Number { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// 
		/// [长度：200]
		/// </summary>
		public string Note { get; set; }
	}
}
