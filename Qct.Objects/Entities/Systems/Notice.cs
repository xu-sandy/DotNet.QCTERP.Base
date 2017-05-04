// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：
// 创建时间：2017-04-27
// 描述信息：
// --------------------------------------------------

using Qct.Infrastructure.Data.EntityInterface;
using System;

namespace Qct.Objects.Entities
{
	/// <summary>
	/// 公告用于管理本系统的公告信息
	/// </summary>
	[Serializable]
	public class Notice:CompanyEntity
	{
		/// <summary>
		/// 活动ID
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
		/// 公告主题
		/// [长度：100]
		/// </summary>
		public string Theme { get; set; }

		/// <summary>
		/// 公告内容
		/// [长度：1000]
		/// </summary>
		public string NoticeContent { get; set; }

		/// <summary>
		/// 公告类型（1-公告；2-活动）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short Type { get; set; }

		/// <summary>
		/// 公告状态（ 0:未发布 1:已发布）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short State { get; set; }

		/// <summary>
		/// 创建人UID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }

		/// <summary>
		/// 创建时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 公告范围（门店ID或供应商ID，多个以逗号间隔）（0:后台系统）
		/// [长度：4000]
		/// [不允许为空]
		/// </summary>
		public string StoreId { get; set; }

		/// <summary>
		/// 截止日期（Date）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public DateTime ExpirationDate { get; set; }

		/// <summary>
		/// 开始日期（Date）
		/// [长度：10]
		/// </summary>
		public DateTime BeginDate { get; set; }

		/// <summary>
		/// 主题图片 Url
		/// [长度：200]
		/// </summary>
		public string Url { get; set; }
	}
}
