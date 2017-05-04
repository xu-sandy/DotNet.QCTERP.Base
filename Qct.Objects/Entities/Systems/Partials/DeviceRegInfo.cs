using Qct.Infrastructure.Data.EntityInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.Entities.Systems
{
    public partial class DeviceRegInfo : CompanyEntity, IntegerIdEntity
    {

        public  virtual SysUserInfo Auditor { get; set; }
        public  virtual Warehouse Store { get; set; }
    }
}
