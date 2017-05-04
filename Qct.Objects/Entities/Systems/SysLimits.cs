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
	/// 用于管理本系统的全局权限Code信息
	/// </summary>
	public class SysLimits:CompanyEntity
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
		/// 权限名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 权限 ID （全局唯一）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public int LimitId { get; set; }

		/// <summary>
		/// 隶属权限 ID（ 0:顶级）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int PLimitId { get; set; }

		/// <summary>
		/// 深度（1-9）
		/// [长度：5]
		/// </summary>
		public short Depth { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// 状态（0:关闭/停用、1:显示/默认选中、2:可选）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((2))]
		/// </summary>
		public short Status { get; set; }
	}
}
