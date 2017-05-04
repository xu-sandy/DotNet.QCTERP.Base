using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.ValueObjects
{
    public enum OrderState:short
    {
        未提交 = -1,
        未审核 = 0,
        未配送 = 1,
        配送中 = 2,
        已中止 = 3,
        已配送 = 4,
        已收货 = 5
    }
}
