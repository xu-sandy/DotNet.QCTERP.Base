using Qct.Infrastructure.Exceptions;

namespace Qct.Services.Exceptions
{
    public class NotFoundProductException : QCTException
    {
        public NotFoundProductException(string msg) : base(msg)
        {

        }
    }
}
