using Qct.Infrastructure.Json;
using SuperSocket.ProtoBase;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Qct.Infrastructure.Net.SocketClient
{
    public class SockectPackageMessage : PackageInfo<string, byte[]>
    {
        public SockectPackageMessage(string route, byte[] parameter) : base(route, parameter) { }
        public Stream GetStream()
        {
            return GetStream(Body, 0, Body.Length);
        }
        public Stream GetStream(byte[] buffer, int index, int count)
        {
            Stream s = new MemoryStream(buffer, index, count);
            return s;
        }
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
            catch (Exception)
            {
                result = string.Empty;
                return false;
            }
        }
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
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

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
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }
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
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }
        public bool TryReadFromTextStream(out string result, Encoding encoding = default(Encoding))
        {
            try
            {
                if (encoding == default(Encoding))
                {
                    encoding = Encoding.Default;
                }
                using (var s = GetStream())
                {
                    StreamReader reader = new StreamReader(s, encoding);
                    result = reader.ReadToEnd();
                    reader.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                result = string.Empty;
                return false;
            }
        }
    }
}
