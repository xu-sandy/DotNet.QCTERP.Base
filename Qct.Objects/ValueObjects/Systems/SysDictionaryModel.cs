using Qct.Objects.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qct.Objects.ValueObjects
{
    [NotMapped]
    public class SysDictionaryModel : SysDataDictionary
    {
        /// <summary>
        /// 显示的子项字符串
        /// </summary>
        public string ItemsStr { get; set; }
        /// <summary>
        /// 所有子项树
        /// </summary>
        public int ItemsCount { get; set; }
        /// <summary>
        /// 父级字典名称
        /// </summary>
        public string PTitle { get; set; }
    }
}
