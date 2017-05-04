// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：linbl
// 创建时间：2017-04-18
// 描述信息：
// --------------------------------------------------

using Qct.Infrastructure.Data.EntityInterface;
using System;

namespace Qct.Objects.Entities
{
    /// <summary>
    /// 用于管理本系统的所有业务数据字典信息
    /// </summary>
    public  partial class  SysDataDictionary: CompanyEntity
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
        /// 编号（该编号全局唯一）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int DicSN { get; set; }

        /// <summary>
        /// 父编号ID（0：顶级）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int DicPSN { get; set; }

        /// <summary>
        /// 排序（0:无）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 类别名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 状态（0:关闭、1:可用）
        /// [长度：1]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public bool Status { get; set; }
    }

}
