using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;

namespace Qct.Infrastructure.Net.SocketClient
{
    public interface IPackageHandler
    {
        string Route { get; set; } 
    }
    public interface IPackageHandler<TClient, TPackage> : IPackageHandler
        where TClient : EasyClientBase
        where TPackage : IPackageInfo
    {
        void RemoteCallback(TClient client, TPackage package);
    }
    public interface ISocketPackageHandler : IPackageHandler<SocketClient, SockectPackageMessage>, IPackageHandler
    {

    }
}
