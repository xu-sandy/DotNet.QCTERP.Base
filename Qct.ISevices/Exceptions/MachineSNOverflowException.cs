using Qct.Infrastructure.Exceptions;

namespace Qct.ISevices.Exceptions
{
    public class MachineSNOverflowException : QCTException
    {
        public MachineSNOverflowException(string msg) : base(msg)
        {
        }
    }
}
