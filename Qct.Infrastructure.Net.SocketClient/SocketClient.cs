using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Qct.Infrastructure.Net.SocketClient.Extensions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Qct.Infrastructure.Json;

namespace Qct.Infrastructure.Net.SocketClient
{
    public class SocketClient : EasyClient
    {
        public SocketClient(IRouteProvider routeProvider,params Assembly[] cmdAssemblies)
        {
            RouteProvider = routeProvider;
            //加载处理程序
            var commandAssemblies = cmdAssemblies;
            foreach (var assembly in commandAssemblies)
            {
                try
                {
                    lock (ResponseHandlers)
                    {
                        ResponseHandlers.AddRange(assembly.GetImplementedObjectsByInterface<ISocketPackageHandler>());
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(string.Format("加载程序集失败，程序集： {0}!", assembly.FullName), exc);
                }
            }
        }
        public IRouteProvider RouteProvider { get; private set; }

        protected internal List<ISocketPackageHandler> ResponseHandlers = new List<ISocketPackageHandler>();

        public virtual void Initialize()
        {
            Initialize(new DefaultRouteReceiveFilter(RouteProvider), (response) =>
            {
                ISocketPackageHandler handler;
                lock (ResponseHandlers)
                {
                    handler = ResponseHandlers.FirstOrDefault(o => o.Route == response.Key);
                }
                if (handler != null)
                {
                    handler.RemoteCallback(this, response);
                }
            });
        }

        public byte[] Format(byte[] route, byte[] msg)
        {
            var len = BitConverter.GetBytes(msg.Length);
            var rawMsg = new byte[route.Length + len.Length + msg.Length];

            Array.Copy(route, 0, rawMsg, 0, route.Length);
            Array.Copy(len, 0, rawMsg, route.Length, len.Length);
            if (msg.LongLength > 0)
                Array.Copy(msg, 0, rawMsg, route.Length + len.Length, msg.Length);

            return rawMsg;
        }

        public SockectPackageMessage SendBytesWithResponse(byte[] route, byte[] body = null)
        {
            return new RequsetWithResponseHandler(this, route).Request(body);
        }

        public void SendBytes(byte[] route, byte[] body = null)
        {
            if (route == null || route.Length != RouteProvider.RouteLength)
            {
                throw new ArgumentNullException("路由码与路由不匹配，请确认路由码长度！");
            }
            if (!IsConnected)
            {
                throw new Exception("未连接到服务器不能发送数据！");
            }
            if (body == null)
            {
                body = new byte[0];
            }
            var content = Format(route, body);
            Send(new ArraySegment<byte>(content));
        }
        public SockectPackageMessage SendMemoryStreamWithResponse(byte[] route, MemoryStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("数据流不能为null！");

            var body = stream.ToArray();
            stream.Close();
            return new RequsetWithResponseHandler(this, route).Request(body);
        }
        public void SendMemoryStream(byte[] route, MemoryStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("数据流不能为null！");

            var body = stream.ToArray();
            stream.Close();
            // Send data to the server
            SendBytes(route, body);
        }
        public SockectPackageMessage SendObjectToBinaryStreamWithResponse(byte[] route, object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("发送内容不能为null！");
            }
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            return SendMemoryStreamWithResponse(route, ms);
        }
        public void SendObjectToBinaryStream(byte[] cmdCode, object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("发送内容不能为null！");
            }
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            SendMemoryStream(cmdCode, ms);
        }
        public SockectPackageMessage SendTextToStreamWithResponse(byte[] route, string text, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("发送内容不能为空！");
            }
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, encoding);
            sw.Write(text);
            var result = SendMemoryStreamWithResponse(route, ms);
            sw.Close();
            return result;
        }
        public void SendTextToStream(byte[] route, string text, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("发送内容不能为空！");
            }
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, encoding);
            sw.Write(text);
            SendMemoryStream(route, ms);
            sw.Close();
        }
        public void SendObjectToXMLStream(byte[] route, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("发送内容不能为null！");
            MemoryStream ms = new MemoryStream();
            XmlSerializer formatter = new XmlSerializer(obj.GetType());
            formatter.Serialize(ms, obj);
            SendMemoryStream(route, ms);
        }
        public SockectPackageMessage SendObjectToXMLStreamWithResponse(byte[] route, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("发送内容不能为null！");
            MemoryStream ms = new MemoryStream();
            XmlSerializer formatter = new XmlSerializer(obj.GetType());
            formatter.Serialize(ms, obj);
            var result = SendMemoryStreamWithResponse(route, ms);
            return result;
        }
        public void SendObjectToJsonStream(byte[] route, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("发送内容不能为null！");

            using (MemoryStream ms = new MemoryStream())
            {
                using (var sw = JsonHelper.ToJsonStream(ms, obj))
                {
                    SendMemoryStream(route, ms);
                }
            }
        }
        public SockectPackageMessage SendObjectToJsonStreamWithResponse(byte[] route, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("发送内容不能为null！");

            using (MemoryStream ms = new MemoryStream())
            {
                using (var sw = JsonHelper.ToJsonStream(ms, obj))
                {
                    var result = SendMemoryStreamWithResponse(route, ms);
                    return result;
                }
            }
        }
    }
}
