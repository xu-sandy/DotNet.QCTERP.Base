using Qct.Infrastructure.Json;
using SuperSocket.SocketBase;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Qct.Infrastructure.Net.SocketServer
{
    /// <summary>
    /// socket 会话
    /// </summary>
    public class SocketSession : AppSession<SocketSession, SockectRequestMessage>
    {
        /// <summary>
        /// socket 服务器对象
        /// </summary>
        public SocketServer SocketServer { get { return AppServer as SocketServer; } }
        /// <summary>
        /// 数据协议格式化
        /// </summary>
        /// <param name="route">规则提供程序</param>
        /// <param name="msg">消息体</param>
        /// <returns>消息信息体</returns>
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
        /// <summary>
        /// 将文本转成消息体
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="encoding">编码</param>
        /// <returns>消息信息体</returns>
        public byte[] TextToBytes(string text, Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
            {
                encoding = Encoding.Default;
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("发送内容不能为空！");
            }
            var bytes = encoding.GetBytes(text);
            return bytes;
        }
        /// <summary>
        /// 将对象经过JSON序列化转成字符串
        /// </summary>
        /// <param name="obj">需转化对象</param>
        /// <param name="settings">Json.net序列化设置对象</param>
        /// <returns>json字符串</returns>
        public string ObjectToJsonString(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("发送内容不能为null！");
            return JsonHelper.ToJson(obj);
        }
        /// <summary>
        /// 以Json形式发送对象到数据流中
        /// </summary>
        /// <param name="cmdCode">路由码</param>
        /// <param name="obj">需转化对象</param>
        /// <param name="settings">Json.net序列化设置对象</param>
        public void SendObjectToJsonStream(byte[] cmdCode, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("发送内容不能为null！");

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = JsonHelper.ToJsonStream(ms, obj))
                {
                    SendMemoryStream(cmdCode, ms);
                }
            }
        }
        /// <summary>
        ///  以XML形式发送对象到数据流中
        /// </summary>
        /// <param name="cmdCode">路由码</param>
        /// <param name="obj">对象</param>
        public void SendObjectToXMLStream(byte[] cmdCode, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("发送内容不能为null！");
            MemoryStream ms = new MemoryStream();
            XmlSerializer formatter = new XmlSerializer(obj.GetType());
            formatter.Serialize(ms, obj);
            SendMemoryStream(cmdCode, ms);
        }
        /// <summary>
        ///  以文本形式发送到数据流中
        /// </summary>
        /// <param name="cmdCode">路由码</param>
        /// <param name="text">文本</param>
        /// <param name="encoding">编码</param>
        public void SendTextToStream(byte[] cmdCode, string text, Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
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
            sw.Flush();
            SendMemoryStream(cmdCode, ms);
        }
        /// <summary>
        ///  以二进制形式发送对象到数据流中
        /// </summary>
        /// <param name="cmdCode">路由码</param>
        /// <param name="obj">需发送对象</param>
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
        /// <summary>
        /// 发送比特数据-
        /// </summary>
        /// <param name="cmdCode">路由码</param>
        /// <param name="body">比特数据</param>
        public void SendBytes(byte[] cmdCode, byte[] body = null)
        {
            if (cmdCode == null || cmdCode.Length != SocketServer.RouteProvider.RouteLength)
            {
                throw new ArgumentNullException("路由码与路由不匹配，请确认路由码长度！");
            }
            if (body == null)
            {
                body = new byte[0];
            }
            var content = this.Format(cmdCode, body);
            this.Send(new ArraySegment<byte>(content));
        }
        /// <summary>
        /// 发送流数据
        /// </summary>
        /// <param name="cmdCode">路由码</param>
        /// <param name="stream">数据流</param>
        public void SendMemoryStream(byte[] cmdCode, MemoryStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("数据流不能为null！");

            var body = stream.ToArray();
            stream.Close();
            // Send data to the server
            SendBytes(cmdCode, body);
        }
        /// <summary>
        /// 会话初始化
        /// </summary>
        /// <param name="appServer"></param>
        /// <param name="socketSession"></param>
        public override void Initialize(IAppServer<SocketSession, SockectRequestMessage> appServer, ISocketSession socketSession)
        {
            base.Initialize(appServer, socketSession);
        }
        /// <summary>
        /// 会话关闭
        /// </summary>
        /// <param name="reason"></param>
        public override void Close(CloseReason reason)
        {
            base.Close(reason);
        }
        /// <summary>
        /// 会话已开始
        /// </summary>
        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
        }
        /// <summary>
        /// 会话已关闭
        /// </summary>
        /// <param name="reason"></param>
        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);
        }
        /// <summary>
        /// 会话异常截取
        /// </summary>
        /// <param name="e">异常</param>
        protected override void HandleException(Exception e)
        {
            Logger.Error(string.Format("Session({0}) HandleException", SessionID), e);
            base.HandleException(e);
        }
    }
}
