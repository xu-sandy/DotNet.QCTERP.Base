using Qct.Infrastructure.Exceptions;

namespace Qct.Objects.Exceptions
{
    public class MembershipCardStateFailureException : QCTException
    {
        public MembershipCardStateFailureException(string msg) : base(msg) { }
    }
}

