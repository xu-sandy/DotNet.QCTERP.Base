using Qct.Infrastructure.Data.EntityInterface;
using System.Collections.Generic;

namespace Qct.Objects.Entities
{
    public partial class ProductChangePrice : IntegerIdEntity
    {
        /// <summary>
        /// 调价单项目列表
        /// </summary>
        public virtual ICollection<ProductChangePriceItem> Items { get; set; }
    }
}
