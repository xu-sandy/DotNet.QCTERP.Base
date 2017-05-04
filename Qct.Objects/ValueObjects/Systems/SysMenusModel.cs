// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：ywb
// 创建时间：2015-07-16
// 描述信息：系统菜单信息Model
// --------------------------------------------------

using Newtonsoft.Json;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Qct.Objects.ValueObjects
{
	/// <summary>
	/// 菜单信息
	/// </summary>
    [NotMapped]
    public class SysMenusModel : SysMenus
	{
        /// <summary>
        /// 父级菜单名称
        /// </summary>
        public string PTitle { get; set; }

        /// <summary>
        /// 子级菜单集合
        /// </summary>
        [JsonProperty("children")]
        public List<SysMenusModel> Childs { get; set; }
    }
}
