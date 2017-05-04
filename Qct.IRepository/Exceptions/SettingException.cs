using Qct.Infrastructure.Exceptions;

namespace Qct.IRepository.Exceptions
{
    public class SettingException : QCTException
    {
        public SettingException(string msg) : base(msg) { }
    }
}
