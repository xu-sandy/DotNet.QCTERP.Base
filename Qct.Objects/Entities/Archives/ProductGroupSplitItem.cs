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
	public partial class ProductGroupSplitItem
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
		public int GroupSplitId { get; set; }

		/// <summary>
		/// 子货号
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string ChildProductCode { get; set; }

		/// <summary>
		/// 组合或拆分数量
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
		public decimal Number { get; set; }
	}
}
