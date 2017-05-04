namespace Qct.Infrastructure.MessageClient.ObjectModels
{
    public class SocketResult<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public T Content { get; set; }

        public static SocketResult<T> Create(string code = "200", string message = "", T content = default(T))
        {
            return new SocketResult<T>() { Code = code, Content = content, Message = message };
        }
    }
}
