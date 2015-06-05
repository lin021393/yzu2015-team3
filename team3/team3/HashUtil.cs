using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace team3
{
    class HashUtil
    {
        private static string toHexString(byte[] buffer)
        {
            StringBuilder sb = new StringBuilder(buffer.Length * 2);

            foreach (byte b in buffer)
                sb.AppendFormat("{0:x2}", b);
            
            return sb.ToString();
        }

        public static string EncryptoMD5(string Input)
        {

            MD5 md5 = MD5.Create();
            byte[] source = Encoding.Default.GetBytes(Input);
            byte[] crypto = md5.ComputeHash(source);

            string result = toHexString(crypto);

            return result;
        }

        public static string EncryptoSHA1(string Input)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] source = Encoding.Default.GetBytes(Input);
            byte[] crypto = sha1.ComputeHash(source);

            string result = toHexString(crypto);

            return result;
        }

        public static string EncryptoSHA256(string Input)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] source = Encoding.Default.GetBytes(Input);
            byte[] crypto = sha256.ComputeHash(source);

            string result = toHexString(crypto);

            return result;
        }

        public static string EncryptoSHA384(string Input)
        {
            SHA384 sha384 = SHA384.Create();
            byte[] source = Encoding.Default.GetBytes(Input);
            byte[] crypto = sha384.ComputeHash(source);

            string result = toHexString(crypto);

            return result;
        }

        public static string EncryptoSHA512(string Input)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] source = Encoding.Default.GetBytes(Input);
            byte[] crypto = sha512.ComputeHash(source);

            string result = toHexString(crypto);

            return result;
        }
    }
}
