using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Lib.Extensions
{
    public static class CrytpoExtensions
    {
        public static string GetMd5Hash(this string plainText)
        {
            // Convert the original password to bytes; then create the hash
            MD5 md5 = new MD5CryptoServiceProvider();
            var originalBytes = Encoding.Default.GetBytes(plainText);
            var encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
