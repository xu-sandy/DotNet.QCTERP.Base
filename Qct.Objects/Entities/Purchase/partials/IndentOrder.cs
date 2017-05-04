using Qct.Infrastructure.Data.EntityInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.Entities
{
    public partial class IndentOrder:CompanyEntity
    {
        public ICollection<IndentOrderList> IndentOrderItems { get; set; }
    }
}
