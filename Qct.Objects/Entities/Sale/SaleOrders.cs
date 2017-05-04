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
	/// 用于管理本系统的所有商品销售单信息
	/// </summary>
	public partial class SaleOrders
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
		/// 门店ID
		/// [长度：3]
		/// [不允许为空]
		/// </summary>
		public string StoreId { get; set; }

		/// <summary>
		/// POS机号
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string MachineSN { get; set; }

		/// <summary>
		/// 流水号（全局唯一）
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string PaySN { get; set; }

		/// <summary>
		/// 客户自定义流水号
		/// [长度：50]
		/// </summary>
		public string CustomOrderSn { get; set; }

		/// <summary>
		/// 退单原单流水号
		/// [长度：50]
		/// </summary>
		public string ReFundOrderCustomOrderSn { get; set; }

		/// <summary>
		/// 
		/// [长度：19，小数位数：4]
		/// [默认值：((0))]
		/// </summary>
		public decimal ProductCount { get; set; }

		/// <summary>
		/// 金额合计（优惠后)
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal TotalAmount { get; set; }

		/// <summary>
		/// 手动整单折扣金额
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal OrderDiscount { get; set; }

		/// <summary>
		/// 优惠合计
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal PreferentialPrice { get; set; }

		/// <summary>
		/// 支付方式ID（多个ID以,号间隔）没有支付为-1；
		/// [长度：100]
		/// [不允许为空]
		/// </summary>
		public string ApiCode { get; set; }

		/// <summary>
		/// 交易时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 收银员UID 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }

		/// <summary>
		/// 退单日期
		/// [长度：23，小数位数：3]
		/// </summary>
		public DateTime ReturnDT { get; set; }

		/// <summary>
		/// 导购员UID
		/// [长度：40]
		/// </summary>
		public string Salesman { get; set; }

		/// <summary>
		/// 备注
		/// [长度：200]
		/// </summary>
		public string Memo { get; set; }

		/// <summary>
		/// 会员ID
		/// [长度：40]
		/// </summary>
		public string MemberId { get; set; }

		/// <summary>
		/// 账单类型(0：正常销售；1：换货；2：退货,3:退单) 默认值：0
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short Type { get; set; }

		/// <summary>
		/// 状态（默认：0，0：正常，1：退整单）
		/// [长度：10]
		/// [默认值：((0))]
		/// </summary>
		public int State { get; set; }

		/// <summary>
		/// 
		/// [长度：4000]
		/// </summary>
		public string ReturnId { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// </summary>
		public string ReturnOrderUID { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public int ActivityId { get; set; }

		/// <summary>
		/// 抹零后收款金额
		/// [长度：19，小数位数：4]
		/// </summary>
		public decimal Receive { get; set; }

		/// <summary>
		/// 0:正常数据;1:练习模式数据
		/// [长度：1]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public bool IsTest { get; set; }

		/// <summary>
		/// 订单库存状态（0：未处理，1、已处理销售单（包含销售、退货、换货新生成的订单）、2、已退单处理）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short InInventory { get; set; }

		/// <summary>
		/// 记录是否已处理（0:未处理，1：已处理）
		/// [长度：1]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public bool IsProcess { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// </summary>
		public int Reason { get; set; }

	}
}
