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
	/// 用于管理本系统的部门或用户自定义菜单配置信息
	/// </summary>
	public class SysCustomMenus:CompanyEntity
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
		/// 适用类型（-1:全部,1:部门,2:角色,3:用户）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public int Type { get; set; }

		/// <summary>
		/// 适用对象ID（-1:全部,部门ID、角色ID、用户ID）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public int ObjId { get; set; }

		/// <summary>
		/// 拥有菜单项（多个间用,号间隔，-1:继承所在部门）（来自SysMenus表Id）
		/// [长度：-1]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public string MenuIds { get; set; }

		/// <summary>
		/// 排序
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public int SortOrder { get; set; }
	}
}
