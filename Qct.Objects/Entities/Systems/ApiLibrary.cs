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
	/// 集成接口库用于管理本系统的所有集成外部接口信息（如：银联支付接口、扫码支付接口）
	/// </summary>
	[Serializable]
	public class ApiLibrary :CompanyEntity
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
		/// 接口类型（ 1:支付接口）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short ApiType { get; set; }

		/// <summary>
		/// 接口名称
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 接口代码（全局唯一）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int ApiCode { get; set; }

		/// <summary>
		/// 接口地址
		/// [长度：500]
		/// </summary>
		public string ApiUrl { get; set; }

		/// <summary>
		/// 接口ICON
		/// [长度：200]
		/// </summary>
		public string ApiIcon { get; set; }

		/// <summary>
		/// 接口顺序
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public int ApiOrder { get; set; }

		/// <summary>
		/// 备注
		/// [长度：200]
		/// </summary>
		public string Memo { get; set; }

		/// <summary>
		/// 状态（ 0:禁用、 1:可用）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public short State { get; set; }

		/// <summary>
		/// 请求方式[1:post、2:get]
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short ReqMode { get; set; }

		/// <summary>
		/// Token
		/// [长度：100]
		/// </summary>
		public string ApiToken { get; set; }

		/// <summary>
		/// 账号
		/// [长度：50]
		/// </summary>
		public string ApiAccount { get; set; }

		/// <summary>
		/// 密码
		/// [长度：50]
		/// </summary>
		public string ApiPwd { get; set; }

		/// <summary>
		/// 关闭接口 ICON
		/// [长度：200]
		/// </summary>
		public string ApiCloseIcon { get; set; }

	}
}
