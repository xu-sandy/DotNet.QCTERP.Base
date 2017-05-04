using Qct.Infrastructure.Exceptions;

namespace Qct.Services.Exceptions
{
    public class NetException : QCTException
    {
        public NetException(string msg) : base(msg)
        {
        }
    }
}
