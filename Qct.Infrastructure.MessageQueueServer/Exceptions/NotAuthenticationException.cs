namespace Qct.Infrastructure.MessageServer.Exceptions
{
    /// <summary>
    /// 未授权异常
    /// </summary>
    public class NotAuthenticationException : MQMException
    {
        public NotAuthenticationException() : this("501", "未授权操作，请先进行授权！")
        {
        }

        public NotAuthenticationException(string message) : this("501", message) { }


        public NotAuthenticationException(string code, string message) : base(code, message) { }
    }
}
