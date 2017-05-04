using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.ValueObjects
{
    [NotMapped]
    public class IndentOrderItemModel : IndentOrderList
    {
        public decimal InboundNumber { get; set; }
        public string SubUnit { get; set; }
        public string Title { get; set; }
        public string Barcode { get; set; }
        public int CategorySN { get; set; }
        public short Expiry { get; set; }

        public string Detail { get; set; }
        public string Gift { get; set; }
    }
}
