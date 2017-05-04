using SuperSocket.ProtoBase;

namespace Qct.Infrastructure.Net.SocketClient
{
    public static class IBufferStreamExtensions
    {
        public static byte[] Read(this IBufferStream bufferStream)
        {
            var buffer = new byte[bufferStream.Length];

            bufferStream.Read(buffer, 0, (int)bufferStream.Length);
            return buffer;
        }
    }
}
