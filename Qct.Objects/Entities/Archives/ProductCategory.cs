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
	/// 用于管理本系统的所有相关的附件基本信息
	/// </summary>
	public partial class ProductCategory
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
		/// 分类编号（全局唯一）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int CategorySN { get; set; }

		/// <summary>
		/// 上级分类SN
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int CategoryPSN { get; set; }

		/// <summary>
		/// 分类代码 （最大 99，同一级分类下唯一）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short CategoryCode { get; set; }

		/// <summary>
		/// 分类名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 顺序
		/// [长度：10]
		/// </summary>
		public int OrderNum { get; set; }

		/// <summary>
		/// 状态（0:禁用、1:可用）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short State { get; set; }

		/// <summary>
		/// 批发价加价率
		/// [长度：5]
		/// [默认值：((150))]
		/// </summary>
		public short TradePriceRate { get; set; }

		/// <summary>
		/// 销售价加价率
		/// [长度：5]
		/// [默认值：((150))]
		/// </summary>
		public short SalePriceRate { get; set; }

		/// <summary>
		/// 加盟价加价率
		/// [长度：5]
		/// [默认值：((150))]
		/// </summary>
		public short JoinPriceRate { get; set; }

		/// <summary>
		/// 是否零售
		/// [长度：1]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public bool InStoreSale { get; set; }
	}
}
