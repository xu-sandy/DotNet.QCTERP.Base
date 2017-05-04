namespace Qct.Infrastructure.MessageServer
{
    public interface IClientChannel
    {
        bool SendMessage(string message);
    }
}
