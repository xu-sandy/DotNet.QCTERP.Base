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
	public partial class ProductGroupSplit
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
		/// 父货号
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string ParentProductCode { get; set; }

		/// <summary>
		/// 1-折分，2-组合
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short Nature { get; set; }

		/// <summary>
		/// 
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }

		/// <summary>
		/// 
		/// [长度：200]
		/// </summary>
		public string Note { get; set; }
	}
}
