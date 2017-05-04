using Qct.Infrastructure.Exceptions;

namespace Qct.Exceptions
{
    public class OrderException : QCTException
    {
        public OrderException(string msg) : base(msg) { }
    }
}
