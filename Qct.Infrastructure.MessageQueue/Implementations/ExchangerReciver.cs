using Qct.Infrastructure.Net.SocketClient;
using System.Threading.Tasks;

namespace Qct.Infrastructure.MessageClient.Implementations
{
    public class ExchangerReciver : ISocketPackageHandler
    {
        public ExchangerReciver()
        {
            Route = new DefaultRouteProvider(4).GetRoute(new byte[] { 0x01, 0x00, 0x00, 0x05 });
        }
        public string Route { get; set; }

        public void RemoteCallback(SocketClient client, SockectPackageMessage package)
        {
            EventJsonWrapper eventJson;
            if (package.TryReadFromJsonStream(out eventJson))
            {

                var publisher = PublisherFactory.Create();
                if (publisher.PublisherId != eventJson.PublisherId)
                {
                     publisher.PublishAsync(eventJson);
                  
                }
            }
        }
    }
}
