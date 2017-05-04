using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.ValueObjects
{
    public class LoginDnsModel
    {
        //商户号
        public string Cid { get; set; }
        //门店ID
        public string Sid { get; set; }
        //消息
        public string Message { get; set; }
    }
}
