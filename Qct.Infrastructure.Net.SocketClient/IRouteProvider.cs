namespace Qct.Infrastructure.Net.SocketClient
{
    public interface IRouteProvider
    {
        string GetRoute(byte[] routeCode);
        int RouteLength { get; }
    }
}
