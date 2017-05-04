// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：
// 创建时间：2015-07-29
// 描述信息：
// --------------------------------------------------

using Qct.Infrastructure.Data.EntityInterface;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qct.Objects.Entities
{
	/// <summary>
	/// 用于管理本系统的权限组信息
	/// </summary>
    public partial class SysRoles
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        public int Id { get; set; }
        public int CompanyId { get; set; }
		/// <summary>
		/// 角色组 ID（全局唯
		/// [长度：40]
		/// </summary>
        public int RoleId { get; set; }

		/// <summary>
		/// 组名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
        public string Title { get; set; }

		/// <summary>
		/// 权限 ID （多个间用,号间隔， -1:不限）
		/// [长度：4000]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
        public string LimitsIds { get; set; }

		/// <summary>
		/// 描述说明
		/// [长度：100]
		/// </summary>
        public string Memo { get; set; }

		/// <summary>
		/// 状态（0:关闭、1:可用）
		/// [长度：1]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 允许显示到页面 （0: 否、1: 是）
        /// </summary>
        public short ShowView { get; set; }

    }
}
