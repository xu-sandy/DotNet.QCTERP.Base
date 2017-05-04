using Qct.Infrastructure.Exceptions;

namespace Qct.Services.Exceptions
{
    public class UnauthorizedException : QCTException
    {
        public UnauthorizedException(string msg) : base(msg)
        {

        }
    }
}