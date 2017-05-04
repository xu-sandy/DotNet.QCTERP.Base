using Qct.Objects.ValueObjects.Systems;
using System;

namespace Qct.Objects.ValueObjects
{
    public class DeviceModel
    {
        public int Id { get; set; }
        public string StoreId { get; set; }
        public DeviceType Type { get; set; }
        public string DeviceSN { get; set; }
        public string MachineSN { get; set; }
        public string Store { get; set; }
        public DateTime CreateDT { get; set; }
        public DeviceState State { get; set; }
        public string Auditor { get; set; }
        public string Memo { get; set; }
    }
}
