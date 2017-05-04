using Qct.Infrastructure.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Qct.Infrastructure.Net.Http.RestClient
{
    public class RestRequest<R> : IDisposable
   where R : class
    {
        private WebRequest _request = null;
        private HttpWebRequest request = null;
        private HttpWebResponse response = null;
        Stream requestStream = null;
        StreamWriter requestWriter = null;
        Stream responseStream = null;
        StreamReader responseReader = null;
        public RestRequest(RequestSetting setting)
        {
            Setting = setting;
            var queryString = GetQueryString(setting);
            UriBuilder ub = new UriBuilder(setting.Schema, setting.Host, setting.Port, setting.Path, queryString);
            Uri = ub.Uri;
            if (Uri.Scheme.ToLower() == "https")
            {
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidate;
            }
            _request = WebRequest.Create(Uri);
            request = _request as HttpWebRequest;
            _request.Method = setting.Method;
            _request.ContentType = setting.ContentType;
            _request.Timeout = setting.Timeout;
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";
            SetHeaders(setting.Headers);
            if (request != null)
            {
                SetRequsetCookie(null);
            }

        }

        public void SetHeaders(Dictionary<HttpRequestHeader, string> headers)
        {
            foreach(var item in headers)
            {
                request.Headers.Set(item.Key, item.Value);
            }
        }
        /// <summary>
        /// 设置安全协议
        /// </summary>
        /// <param name="type"></param>
        public void SetSecurityProtocol(SecurityProtocolType type)
        {
            if (Uri.Scheme.ToLower().EndsWith("s"))
            {
                ServicePointManager.SecurityProtocol = type;
            }
        }
        /// <summary>
        /// 请求设置
        /// </summary>
        public RequestSetting Setting { get; private set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public Uri Uri { get; private set; }
        /// <summary>
        /// 设置请求的cookie
        /// </summary>
        /// <param name="cookie"></param>
        public void SetRequsetCookie(CookieContainer cookie)
        {
            if (cookie != null)
            {
                request.CookieContainer = cookie;
            }
            else
            {
                request.CookieContainer = new CookieContainer();
            }
        }
        /// <summary>
        /// 获取返回的cookie
        /// </summary>
        /// <returns></returns>
        public CookieContainer GetResponeCookie()
        {
            if (response != null)
            {
                var result = new CookieContainer();
                if (response.Cookies.Count == 0)
                {
                    string cookies = response.Headers["Set-Cookie"];
                    if (!string.IsNullOrEmpty(cookies))
                    {
                        result.Add(parseCookies(cookies));
                    }
                }
                else
                {
                    result.Add(response.Cookies);
                }
                return result;
            }
            return null;
        }
        /// <summary>
        /// 获取Url参数
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        protected virtual string GetQueryString(RequestSetting setting)
        {
            string result = string.Empty;
            if (setting.UriParameters != null && setting.UriParameters.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("?");
                foreach (var item in setting.UriParameters)
                {
                    sb.AppendFormat("{0}={1}&", item.Key, item.Value);
                }
                result = sb.ToString();
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
        /// <summary>
        /// 处理远程证书验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
        /// <summary>
        /// 解析cookie
        /// </summary>
        /// <param name="cookieHeader"></param>
        /// <returns></returns>
        private CookieCollection parseCookies(string cookieHeader)
        {
            CookieCollection cc = new CookieCollection();
            var cookies = cookieHeader.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            Cookie cookie = null;
            for (int k = 0, len = cookies.Length; k < len; k++)
            {
                if (cookies[k].TrimStart().StartsWith("expires="))
                {
                    var arr = cookies[k].Trim().Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (cookie != null && arr.Length > 1)
                    {
                        try
                        {
                            cookie.Expires = DateTime.Parse(arr[1]);
                        }
                        catch (Exception)
                        {
                            cookie.Expires = DateTime.Now.AddDays(30);
                        }
                    }
                }
                else if (cookies[k].TrimStart().StartsWith("path="))
                {
                    var arr = cookies[k].Trim().Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (cookie != null && arr.Length > 1)
                    {
                        cookie.Path = arr[1];
                    }
                }
                else
                {
                    if (cookie == null)
                    {
                        cookie = new Cookie();
                        cookie.Domain = Uri.Host;
                        cc.Add(cookie);
                    }
                    var cookieInfo = cookies[k].Trim().Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (cookieInfo.Length > 0)
                    {
                        cookie.Name = cookieInfo[0];
                    }
                    if (cookieInfo.Length > 1)
                    {
                        cookie.Value = cookieInfo[1];
                    }
                }
            }
            return cc;
        }
        /// <summary>
        /// 发起请求
        /// </summary>
        private void DoRequest()
        {
            if (_request.Method != "GET")
            {
                requestStream = _request.GetRequestStream();
                requestWriter = new StreamWriter(requestStream);
                requestWriter.Write(Setting.Content);
                requestWriter.Close();
                requestWriter.Dispose();
                requestWriter = null;
            }
        }
        /// <summary>
        /// 获取返回数据流
        /// </summary>
        /// <returns></returns>
        private Stream GetResponse()
        {
            DoRequest();
            response = _request.GetResponse() as HttpWebResponse;
            responseStream = response.GetResponseStream();
            SetRequsetCookie(GetResponeCookie());
            return responseStream;
        }
        /// <summary>
        /// 返回Json反序列化的对象
        /// </summary>
        /// <returns></returns>
        public virtual R ExecuteWithJsonToObject()
        {
            return JsonHelper.ToObject<R>(ExecuteWithString());
        }
        /// <summary>
        /// 返回Row字符串的数据
        /// </summary>
        /// <returns></returns>
        public virtual string ExecuteWithString()
        {
            var responseStream = GetResponse();
            responseReader = new StreamReader(responseStream);
            var rawResponse = responseReader.ReadToEnd();
            return rawResponse;
        }
        /// <summary>
        /// 返回byte数组的数据
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ExecuteWithBinary()
        {
            var responseStream = GetResponse();
            byte[] bytes = new byte[responseStream.Length];
            responseStream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// 将数据返回保存成文件
        /// </summary>
        /// <param name="fileName"></param>
        public virtual void ExecuteWithFile(string fileName)
        {
            FileStream fs = null;
            BinaryWriter bw = null;
            try
            {
                var bytes = ExecuteWithBinary();
                fs = new FileStream(fileName, FileMode.Create);
                bw = new BinaryWriter(fs);
                bw.Write(bytes);
            }
            finally
            {
                if (bw != null)
                    bw.Close();
                if (fs != null)
                    fs.Close();
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (requestStream != null)
            {
                if (requestWriter != null)
                {
                    requestWriter.Close();
                }
                else
                {
                    requestStream.Close();
                }
                requestStream.Dispose();
                requestStream = null;
            }
            if (responseStream != null)
            {
                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader.Dispose();
                    responseReader = null;
                }
                else
                {
                    responseStream.Close();
                    responseStream.Dispose();
                    responseStream = null;
                }
            }
            if (response != null)
            {
                response.Close();
            }
        }
    }
}
