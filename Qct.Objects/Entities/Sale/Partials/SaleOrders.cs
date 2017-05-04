using Qct.Infrastructure.Data.EntityInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.Entities
{
    public partial class SaleOrders:CompanyEntity
    {
        [ForeignKey("PaySN")]

        public virtual ICollection<ConsumptionPayment> ConsumptionPayments { get; set; }
        [ForeignKey("PaySN")]

        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
        [ForeignKey("PaySN")]
        public virtual ICollection<WipeZero> WipeZeros { get; set; }
    }
}
