using System;

namespace Qct.Infrastructure.Exceptions
{
    public class QCTException : Exception
    {
        public QCTException(string msg) : base(msg)
        {

        }
        public QCTException(string msg,object datas) : base(msg)
        {
            Datas = datas;
        }
        public object Datas { get; set; }
        public string ErrorCode { get; set; }
    }
}
