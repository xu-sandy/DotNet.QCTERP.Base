using Qct.Infrastructure.Data.EntityInterface;
using System;

namespace Qct.Objects.Entities
{
    public partial class StoredValueCardPayRecord : IntegerIdEntity
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDT { get; set; }
        public string OrderSn { get; set; }
        public string CardNo { get; set; }
        public string StoreId { get; set; }
        public string CreateUID { get; set; }
        public byte[] SyncItemVersion { get; set; }
        public Guid SyncItemId { get; set; }

    }
}
