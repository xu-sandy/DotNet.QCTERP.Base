using Qct.Infrastructure.Data.EntityInterface;
using System.ComponentModel.DataAnnotations;

namespace Qct.Objects.Entities
{
    /// <summary>
    /// 自增数
    /// </summary>
    public class IncreasingNumber: StringIdEntity
    {
        /// <summary>
        /// 自增数标识
        /// </summary>
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// 自增数值记录
        /// </summary>
        public int Number { get; set; }
    }
}
