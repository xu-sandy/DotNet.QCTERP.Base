using Qct.Infrastructure.Exceptions;

namespace Qct.Infrastructure.MessageServer.Exceptions
{
    public class MQMException : QCTException
    {
        public MQMException(string code, string message) : base(message)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
}
