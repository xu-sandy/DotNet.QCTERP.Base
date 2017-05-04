using Qct.Infrastructure.Exceptions;

namespace Qct.Repository.Pos.Exceptions
{
    public class NetException : QCTException
    {
        public NetException(string msg) : base(msg)
        {
        }
    }
}
