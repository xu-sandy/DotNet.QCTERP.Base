using Qct.Infrastructure.Exceptions;

namespace Qct.Services.Exceptions
{
    public class BarcodeParseException : QCTException
    {
        public BarcodeParseException(string msg) : base(msg)
        {

        }
    }
}
