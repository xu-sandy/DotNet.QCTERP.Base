using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.Log
{
    public abstract class BaseLogContent
    {
        public BaseLogContent() { }
        public BaseLogContent(Exception ex)
        {
            Summary = FormatMessage(ex);
        }

        public byte Type { get; set; }

        public string Summary { get; set; }

        public string ModuleName { get; set; }

        public virtual string FormatMessage(Exception ex)
        {
            if (ex == null) return "";
            Func<Exception, Exception> ToInnerException = null;
            ToInnerException = (inner) =>
            {
                if (inner.InnerException == null)
                {
                    return inner;
                }
                return ToInnerException(ex.InnerException);
            };
            var e = ToInnerException(ex);
            return (ex.Message == e.Message ? e.Message : ex.Message + "\r\n描述:" + e.Message) + "\r\n源:" + e.Source + "\r\n引发原因:" + e.TargetSite + "\r\n堆栈跟踪:" + e.StackTrace;
        }
    }
}
