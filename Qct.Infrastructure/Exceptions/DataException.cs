namespace Qct.Infrastructure.Exceptions
{
    public class DataException : QCTException
    {
        public DataException(string msg) : base(msg) { }
        public DataException(string msg, object datas) : base(msg, datas) { }
    }
}
