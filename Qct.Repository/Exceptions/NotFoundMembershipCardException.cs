using Qct.Infrastructure.Exceptions;

namespace Qct.Repository.Exceptions
{
    public class NotFoundMembershipCardException : QCTException
    {
        public NotFoundMembershipCardException(string msg) : base(msg)
        {

        }
    }
}