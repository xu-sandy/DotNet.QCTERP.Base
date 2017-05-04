using Qct.Infrastructure.Data.EntityInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.Entities
{
    public partial class ProductGroupSplit : IntegerIdEntity
    {
        public virtual ICollection<ProductGroupSplitItem> Items { get; set; }
    }
}
