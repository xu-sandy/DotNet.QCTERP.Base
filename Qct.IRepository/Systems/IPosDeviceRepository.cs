using Qct.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.IRepository
{
    public interface IPosDeviceRepository
    {
        POSDeviceInformation Get(bool isNotFoundThrowException = false);
        void Save(POSDeviceInformation deviceInfo);
    }
}
