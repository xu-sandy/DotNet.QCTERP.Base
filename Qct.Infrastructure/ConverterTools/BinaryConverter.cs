using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.ConverterTools
{
    public static class BinaryConverter
    {
        public static string ToHexString(this byte[] bin, bool isUpper = true)
        {
            var formatText = "{0:X2}";
            if (!isUpper)
                formatText = "{0:x2}";
            StringBuilder hex = new StringBuilder(bin.Length * 2);
            if (bin != null && bin.Length > 0)
                foreach (byte b in bin)
                    hex.AppendFormat(formatText, b);
            return hex.ToString();
        }

        public static byte[] ToByteArray(this string hex)
        {
            if (string.IsNullOrEmpty(hex))
                throw new ArgumentNullException("hex can not be NULL or EMPTY");
            if (hex.Length % 2 != 0)
                throw new FormatException("hex string length mast be even");
            int byteLen = hex.Length / 2;
            byte[] result = new byte[byteLen];
            for (int i = 0; i < byteLen; i++)
                result[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            return result;
        }
    }
}
