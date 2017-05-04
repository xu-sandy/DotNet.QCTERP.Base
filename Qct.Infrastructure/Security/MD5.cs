using Qct.Infrastructure.ConverterTools;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Qct.Infrastructure.Security
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class MD5
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="normalTxt">明文</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文数据</returns>
        public static byte[] Encrypt(string normalTxt, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(normalTxt))
                return new byte[0];
            if (encoding == null)
                encoding = Encoding.Default;
            var bytes = encoding.GetBytes(normalTxt);//求Byte[]数组  
            using (var md5Provider = new MD5CryptoServiceProvider())
            {
                var result = md5Provider.ComputeHash(bytes);//求哈希值  
                return result;//将Byte[]数组转为净荷明文(其实就是字符串)  
            }
        }
        /// <summary>
        /// MD5加密，返回Hex字符串
        /// </summary>
        /// <param name="normalTxt">明文</param>
        /// <param name="encoding">编码</param>
        /// <param name="isUpper">输出Hex是否为大写字母</param>
        /// <returns>密文</returns>
        public static string EncryptOutputHex(string normalTxt, Encoding encoding = null, bool isUpper = true)
        {
            var encryptBytes = Encrypt(normalTxt);
            var textEncrypt = encryptBytes.ToHexString(isUpper);
            return textEncrypt;
        }
        /// <summary>
        ///  MD5加密，返回base64字符串
        /// </summary>
        /// <param name="normalTxt">明文</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文</returns>
        public static string EncryptOutputBase64String(string normalTxt, Encoding encoding = null)
        {
            var encryptBytes = Encrypt(normalTxt);
            var textEncrypt = Convert.ToBase64String(encryptBytes);
            return textEncrypt;

        }
    }
}
