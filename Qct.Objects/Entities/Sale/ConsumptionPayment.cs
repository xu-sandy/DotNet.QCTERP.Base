// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：
// 创建时间：2017-05-03
// 描述信息：
// --------------------------------------------------

using Qct.Infrastructure.Data.EntityInterface;
using System;

namespace Qct.Objects.Entities
{
	/// <summary>
	/// 消费支付方式用于管理本系统的所有商品销售结算方式信息（主表：SaleOrders）
	/// </summary>
	[Serializable]
	public class ConsumptionPayment:CompanyEntity
	{
		/// <summary>
		/// 记录 ID
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
		/// 流水号
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string PaySN { get; set; }

		/// <summary>
		/// 支付方式 ID
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int ApiCode { get; set; }

		/// <summary>
		/// 交易单号（由支付宝或微信或银联返回）
		/// [长度：50]
		/// </summary>
		public string ApiOrderSN { get; set; }

		/// <summary>
		/// 实收金额
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// 备注
		/// [长度：200]
		/// </summary>
		public string Memo { get; set; }

		/// <summary>
		/// 支付状态（0：未支付、1：已支付）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short State { get; set; }

		/// <summary>
		/// 卡号
		/// [长度：50]
		/// </summary>
		public string CardNo { get; set; }

		/// <summary>
		/// 找零
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal Change { get; set; }

		/// <summary>
		/// 客付金额
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal Received { get; set; }

	}
}
