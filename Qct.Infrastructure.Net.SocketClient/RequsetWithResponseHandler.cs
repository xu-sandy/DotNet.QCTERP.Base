using System.Threading;
using System.Threading.Tasks;

namespace Qct.Infrastructure.Net.SocketClient
{
    internal class RequsetWithResponseHandler : ISocketPackageHandler
    {
        public RequsetWithResponseHandler(SocketClient client, params byte[] routeCode)
        {
            Route = client.RouteProvider.GetRoute(routeCode);
            RouteCode = routeCode;
            Package = null;
            Client = client;
            autoEvent = new AutoResetEvent(false);
        }

        private const int Timeout = 120000;
        private byte[] RouteCode { get; set; }
        AutoResetEvent autoEvent;
        public string Route { get; set; }
        public SocketClient Client { get; set; }

        private SockectPackageMessage Package { get; set; }
        public void RemoteCallback(SocketClient client, SockectPackageMessage package)
        {
            Package = package;
            autoEvent.Set();
        }

        public SockectPackageMessage Request(byte[] body)
        {
            lock (Client.ResponseHandlers)
            {
                Client.ResponseHandlers.Add(this);
            }
            var task = Task.Factory.StartNew(() =>
            {
                if (autoEvent.WaitOne(Timeout) && Package != null)
                {
                    lock (Client.ResponseHandlers)
                    {
                        Client.ResponseHandlers.Remove(this);
                    }
                    return Package;
                }
                else
                {
                    lock (Client.ResponseHandlers)
                    {
                        Client.ResponseHandlers.Remove(this);
                    }
                    return null;
                }

            });
            Client.SendBytes(RouteCode, body);
            task.Wait();
            return task.Result;
        }
    }
}
