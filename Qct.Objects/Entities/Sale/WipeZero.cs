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
	/// 摸零表
	/// </summary>
	[Serializable]
	public class WipeZero
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
		/// 支付编号
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string PaySN { get; set; }

		/// <summary>
		/// 优惠金额
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
		public decimal Number { get; set; }

	}
}
