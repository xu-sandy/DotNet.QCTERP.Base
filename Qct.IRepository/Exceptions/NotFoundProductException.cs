using Qct.Infrastructure.Exceptions;

namespace Qct.IRepository.Exceptions
{
    public class NotFoundProductException : QCTException
    {
        public NotFoundProductException(string msg) : base(msg)
        {
        }
    }
}
