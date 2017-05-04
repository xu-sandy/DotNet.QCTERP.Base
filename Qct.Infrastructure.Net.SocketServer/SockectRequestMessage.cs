using Qct.Infrastructure.Json;
using SuperSocket.SocketBase.Protocol;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Qct.Infrastructure.Net.SocketServer
{
    /// <summary>
    /// socket 会话请求信息
    /// </summary>
    public class SockectRequestMessage : RequestInfo<byte[]>
    {
        /// <summary>
        /// 初始化请求信息
        /// </summary>
        /// <param name="route">路由码</param>
        /// <param name="parameter">消息体</param>
        public SockectRequestMessage(string route, byte[] parameter) : base(route, parameter) { }
        /// <summary>
        /// 消息体长度（bytes）
        /// </summary>
        public int Length { get { return Body.Length; } }
        /// <summary>
        /// 获取缓冲区数据流对象
        /// </summary>
        /// <returns>数据流对象</returns>
        public Stream GetStream()
        {
            return GetStream(Body, 0, Body.Length);
        }
        /// <summary>
        /// 获取缓冲区数据流对象
        /// </summary>
        /// <param name="index">指定索引</param>
        /// <param name="count">指定长度</param>
        /// <returns>数据流对象</returns>
        public Stream GetStream(int index, int count)
        {
            return GetStream(Body, index, count);
        }
        /// <summary>
        /// 获取缓冲区数据流对象
        /// </summary>
        /// <param name="buffer">缓冲区数据</param>
        /// <param name="index">指定索引</param>
        /// <param name="count">指定长度</param>
        /// <returns>数据流对象</returns>
        public Stream GetStream(byte[] buffer, int index, int count)
        {
            Stream s = new MemoryStream(buffer, index, count);
            return s;
        }
        /// <summary>
        /// 获取指定位置和长度的缓冲区数据
        /// </summary>
        /// <param name="index">指定索引</param>
        /// <param name="count">指定长度</param>
        /// <returns>截取的数据</returns>
        public byte[] ReadBuffer(int index, int count)
        {
            return Body.Skip(index).Take(count).ToArray();
        }
        /// <summary>
        /// 将缓冲区数据做JSON序列化为指定对象
        /// </summary>
        /// <typeparam name="T">指定对象类</typeparam>
        /// <param name="result">输出对象</param>
        /// <param name="settings">Json.net序列号设置信息</param>
        /// <returns>是否成功</returns>
        public bool TryReadFromJsonStream<T>(out T result)
        {
            try
            {
                using (var s = GetStream())
                {
                    result = JsonHelper.ToObjectFromStream<T>(s);
                    return true;
                }
            }
            catch (Exception ex)
            {
                result = default(T);
                return false;
            }
        }
        /// <summary>
        /// 将指定区域的缓冲区数据做JSON序列化为指定对象
        /// </summary>
        /// <typeparam name="T">指定对象类</typeparam>
        /// <param name="result">输出对象</param>
        /// <param name="index">指定索引</param>
        /// <param name="count">指定长度</param>
        /// <param name="settings">Json.net序列号设置信息</param>
        /// <returns>是否成功</returns>
        public bool TryReadFromJsonStream<T>(out T result, int index, int count)
        {
            try
            {
                using (var s = GetStream(Body, index, count))
                {
                    result= JsonHelper.ToObjectFromStream<T>(s);
                    return true;
                }
            }
            catch (Exception ex)
            {
                result = default(T);
                return false;
            }
        }
        /// <summary>
        /// 将缓冲区数据做二进制序列化为指定对象
        /// </summary>
        /// <typeparam name="T">指定对象类</typeparam>
        /// <param name="result">指定对象</param>
        /// <returns>是否成功</returns>
        public bool TryReadFromBinaryStream<T>(out T result)
        {
            try
            {
                using (var s = GetStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    result = (T)formatter.Deserialize(s);
                    return true;
                }
            }
            catch (Exception ex)
            {
                result = default(T);
                return false;
            }
        }
        /// <summary>
        /// 将缓冲区数据做XML序列化为指定对象
        /// </summary>
        /// <typeparam name="T">指定对象类</typeparam>
        /// <param name="result">指定对象</param>
        /// <returns>是否成功</returns>
        public bool TryReadFromXMLStream<T>(out T result)
        {
            try
            {
                using (var s = GetStream())
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(T));
                    result = (T)formatter.Deserialize(s);
                    return true;
                }
            }
            catch (Exception ex)
            {
                result = default(T);
                return false;
            }
        }
        /// <summary>
        /// 将缓冲区数据读取为文本
        /// </summary>
        /// <param name="result">文本信息</param>
        /// <param name="encoding">编码</param>
        /// <returns>是否成功</returns>
        public bool TryReadFromTextStream(out string result, Encoding encoding = default(Encoding))
        {
            return TryReadFromText(Body, 0, Body.Length, out result, encoding);
        }
        /// <summary>
        ///  将指定区域的缓冲区数据读取为文本
        /// </summary>
        /// <param name="buffer">缓冲数据</param>
        /// <param name="index">指定索引</param>
        /// <param name="count">指定长度</param>
        /// <param name="result">文本信息</param>
        /// <param name="encoding">编码</param>
        /// <returns>是否成功</returns>
        public bool TryReadFromText(byte[] buffer, int index, int count, out string result, Encoding encoding = default(Encoding))
        {
            try
            {
                if (encoding == default(Encoding))
                {
                    encoding = Encoding.Default;
                }
                using (var s = GetStream(buffer, index, count))
                {
                    StreamReader reader = new StreamReader(s, encoding);
                    result = reader.ReadToEnd();
                    reader.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
                return false;
            }
        }
    }
}
