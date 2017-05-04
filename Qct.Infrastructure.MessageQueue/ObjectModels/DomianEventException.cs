using System;

namespace Qct.Infrastructure.MessageClient.ObjectModels
{
    /// <summary>
    /// 事件异常信息
    /// </summary>
    public class DomianEventException : Exceptions.QCTException
    {
        public DomianEventException(string msg) : base(msg) { }
        public DomianEventException(string msg,object datas) : base(msg, datas) { }
    }
}
