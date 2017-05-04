using System;

namespace Qct.Infrastructure.Net.SocketClient
{
    public class DefaultRouteProvider : IRouteProvider
    {
        public DefaultRouteProvider(int routeLength)
        {
            RouteLength = routeLength;
        }
        public string GetRoute(byte[] routeCode)
        {
            return BitConverter.ToString(routeCode);
        }

        public int RouteLength { get; private set; }
    }
}
