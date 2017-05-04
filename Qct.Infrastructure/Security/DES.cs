using Qct.Infrastructure.ConverterTools;
using Qct.Infrastructure.Exceptions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Qct.Infrastructure.Security
{
    public class DES
    {
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Decrypt(byte[] data, byte[] key, byte[] iv, Encoding encoding = null)
        {
            if (key == null || key.Length != 8)
            {
                throw new DataException("DES密钥不能为空且长度必须为8位！");
            }
            if (iv == null || iv.Length != 8)
            {
                throw new DataException("DES向量不能为空且长度必须为8位！");
            }
            if (data == null || data.Length == 0)
            {
                return string.Empty;
            }
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            dESCryptoServiceProvider.Key = key;
            dESCryptoServiceProvider.IV = iv;
            string result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                }
                var normalBytes = memoryStream.ToArray();
                if (encoding == null)
                    result = Encoding.Default.GetString(normalBytes);
                else
                    result = encoding.GetString(normalBytes);
            }
            return result;
        }

        /// <summary>
        /// DES解密，密文为Hex字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DESDecryptHexString(string data, byte[] key, byte[] iv, Encoding encoding = null)
        {
            var bytes = data.ToByteArray();
            return Decrypt(bytes, key, iv, encoding);
        }
        /// <summary>
        /// DES 加密，返回Hex字符串样式的密文
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DESEncryptHexString(string plaintext, byte[] key, byte[] iv, Encoding encoding = null)
        {
            byte[] datas;
            if (encoding == null)
                datas = Encoding.Default.GetBytes(plaintext);
            else
                datas = encoding.GetBytes(plaintext);
            var result = Encrypt(datas, key, iv);
            return result.ToHexString();
        }
        public static string DESDecryptBase64String(string data, byte[] key, byte[] iv, Encoding encoding = null)
        {
            var bytes = Convert.FromBase64String(data);
            return Decrypt(bytes, key, iv, encoding);
        }
        /// <summary>
        /// DES 加密，返回Base64字符串样式的密文
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DESEncryptBase64String(string plaintext, byte[] key, byte[] iv, Encoding encoding = null)
        {
            byte[] datas;
            if (encoding == null)
                datas = Encoding.Default.GetBytes(plaintext);
            else
                datas = encoding.GetBytes(plaintext);
            var result = Encrypt(datas, key, iv);
            return Convert.ToBase64String(result);
        }
        /// <summary>
        /// DES解密Hex字符串，并将密钥与向量进行MD5运算,转为Hex字符串，取前8位hex字符，作为向量或者密钥
        /// </summary>
        /// <param name="data">解密数据文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <param name="encoding">编码</param>
        /// <returns>明文</returns>
        public static string DESDecryptHexStringWithKeyIVToMd5Hex(string data, string key, string iv, Encoding encoding = null)
        {
            byte[] keyBytes;
            byte[] ivBytes;
            if (encoding == null)
            {
                keyBytes = Encoding.Default.GetBytes(MD5.EncryptOutputHex(key).Substring(0, 8));
                ivBytes = Encoding.Default.GetBytes(MD5.EncryptOutputHex(iv).Substring(0, 8));
            }
            else
            {
                keyBytes = encoding.GetBytes(MD5.EncryptOutputHex(key).Substring(0, 8));
                ivBytes = encoding.GetBytes(MD5.EncryptOutputHex(iv).Substring(0, 8));
            }
            return DESDecryptHexString(data, keyBytes, ivBytes, encoding);
        }
        /// <summary>
        ///  DES解密Hex字符串，并将密钥与向量进行MD5运算,转为base64字符串，取前8位base64字符，作为向量或者密钥
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DESDecryptBase64WithKeyIVToMd5Base64(string data, string key, string iv, Encoding encoding = null)
        {
            byte[] keyBytes;
            byte[] ivBytes;
            if (encoding == null)
            {
                keyBytes = Encoding.Default.GetBytes(MD5.EncryptOutputBase64String(key).Substring(0, 8));
                ivBytes = Encoding.Default.GetBytes(MD5.EncryptOutputBase64String(iv).Substring(0, 8));
            }
            else
            {
                keyBytes = encoding.GetBytes(MD5.EncryptOutputBase64String(key).Substring(0, 8));
                ivBytes = encoding.GetBytes(MD5.EncryptOutputBase64String(iv).Substring(0, 8));
            }
            return DESDecryptBase64String(data, keyBytes, ivBytes, encoding);
        }
        public static string DESEncryptBase64WithKeyIVToMd5Base64(string plaintext, string key, string iv, Encoding encoding = null)
        {
            byte[] datas;
            byte[] keyBytes;
            byte[] ivBytes;
            if (encoding == null)
            {
                keyBytes = Encoding.Default.GetBytes(MD5.EncryptOutputBase64String(key).Substring(0, 8));
                ivBytes = Encoding.Default.GetBytes(MD5.EncryptOutputBase64String(iv).Substring(0, 8));
                encoding = Encoding.Default;
            }
            else
            {
                keyBytes = encoding.GetBytes(MD5.EncryptOutputBase64String(key).Substring(0, 8));
                ivBytes = encoding.GetBytes(MD5.EncryptOutputBase64String(iv).Substring(0, 8));
            }
            datas = encoding.GetBytes(plaintext);

            var result = Encrypt(datas, keyBytes, ivBytes);
            return Convert.ToBase64String(result);
        }
        public static string DESEncryptHexStringWithKeyIVToMd5Hex(string plaintext, string key, string iv, Encoding encoding = null)
        {
            byte[] datas;
            byte[] keyBytes;
            byte[] ivBytes;
            if (encoding == null)
            {
                keyBytes = Encoding.Default.GetBytes(MD5.EncryptOutputHex(key).Substring(0, 8));
                ivBytes = Encoding.Default.GetBytes(MD5.EncryptOutputHex(iv).Substring(0, 8));
                encoding = Encoding.Default;
            }
            else
            {
                keyBytes = encoding.GetBytes(MD5.EncryptOutputHex(key).Substring(0, 8));
                ivBytes = encoding.GetBytes(MD5.EncryptOutputHex(iv).Substring(0, 8));
            }
            datas = encoding.GetBytes(plaintext);
            var result = Encrypt(datas, keyBytes, ivBytes);
            return result.ToHexString();
        }

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (key == null || key.Length != 8)
            {
                throw new DataException("DES密钥不能为空且长度必须为8位！");
            }
            if (iv == null || iv.Length != 8)
            {
                throw new DataException("DES向量不能为空且长度必须为8位！");
            }
            if (data == null || data.Length == 0)
            {
                return new byte[0];
            }
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            dESCryptoServiceProvider.Key = key;
            dESCryptoServiceProvider.IV = iv;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                }
                return memoryStream.ToArray();
            }
        }
    }
}
