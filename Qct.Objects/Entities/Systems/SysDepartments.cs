// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：linbl
// 创建时间：2017-04-20
// 描述信息：
// --------------------------------------------------

using Qct.Infrastructure.Data.EntityInterface;
using System;

namespace Qct.Objects.Entities
{
	/// <summary>
	/// 用于管理本系统的分机构、部门信息
	/// </summary>
	public class SysDepartments:CompanyEntity
	{
		/// <summary>
		/// 机构部门ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 企业标识
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public int CompanyId { get; set; }

		/// <summary>
		/// 类型（1:机构、2:部门、3:子部门）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((2))]
		/// </summary>
		public short Type { get; set; }

		/// <summary>
		/// 机构部门Id(全局唯一)
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int DepId { get; set; }

		/// <summary>
		/// 隶属分机构ID（0:顶级）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public int PDepId { get; set; }

		/// <summary>
		/// 排序（0:无）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// 部门名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 部门代码
		/// [长度：50]
		/// </summary>
		public string SN { get; set; }

		/// <summary>
		/// 部门领导ID（-1:未知）
		/// [长度：40]
		/// [默认值：((-1))]
		/// </summary>
		public string ManagerUId { get; set; }

		/// <summary>
		/// 部门副领导ID（-1:未知）
		/// [长度：40]
		/// [默认值：((-1))]
		/// </summary>
		public string DeputyUId { get; set; }

		/// <summary>
		/// 登录后首页
		/// [长度：200]
		/// </summary>
		public string IndexPageUrl { get; set; }

		/// <summary>
		/// 状态（0:关闭、1:显示）
		/// [长度：1]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public bool Status { get; set; }
	}
}
