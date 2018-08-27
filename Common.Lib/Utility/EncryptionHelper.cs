using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Lib.Utility
{
    public static class EncryptionHelper
    {
        public static class Md5Encryption
        {
            // Hash an input string and return the hash as
            // a 32 character hexadecimal string.
            public static string GetMd5Hash(string strInput)
            {
                // Create a new instance of the MD5CryptoServiceProvider object.
                MD5 md5Hasher = MD5.Create();

                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(strInput));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }

            // Verify a hash against a string.
            public static bool VerifyMd5Hash(string strInput, string strHash)
            {
                // Hash the input.
                string hashOfInput = GetMd5Hash(strInput);

                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, strHash))
                    return true;

                return false;
            }
        }

        public static class SHAEncryption
        {
            // Hash an input string and return the hash as
            // a 32 character hexadecimal string.
            public static string GetSHAHash(string strInput, string sSHAType)
            {
                byte[] data;
                switch (sSHAType.ToUpper())
                {
                    case "SHA1":
                        SHA1 shaM1 = new SHA1Managed();
                        data = shaM1.ComputeHash(Encoding.Default.GetBytes(strInput));
                        break;
                    case "SHA256":
                        SHA256 shaM2 = new SHA256Managed();
                        data = shaM2.ComputeHash(Encoding.Default.GetBytes(strInput));
                        break;
                    case "SHA384":
                        SHA384 shaM3 = new SHA384Managed();
                        data = shaM3.ComputeHash(Encoding.Default.GetBytes(strInput));
                        break;
                    case "SHA512":
                        SHA512 shaM4 = new SHA512Managed();
                        data = shaM4.ComputeHash(Encoding.Default.GetBytes(strInput));
                        break;
                    default:
                        throw new Exception("Could not determine Hash type.\r\n Input value was: " + sSHAType);
                }
                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }

            // Verify a hash against a string.
            public static bool VerifyMd5Hash(string strInput, string strHash, string sSHAType)
            {
                // Hash the input.
                string hashOfInput = GetSHAHash(strInput, sSHAType);

                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, strHash))
                    return true;

                return false;
            }
        }

        public static class SymmetricAlgorithmEncryption
        {
            public static string Encrypt(string plainText, SymmetricAlgorithm symmetricAlgorithm)
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Define cryptographic stream (always use Write mode for encryption).
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                                        symmetricAlgorithm.CreateEncryptor(),
                                                                        CryptoStreamMode.Write))
                    {
                        // Start encrypting.
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                        // Finish encrypting.
                        cryptoStream.FlushFinalBlock();

                        // Convert our encrypted data from a memory stream into a byte array.
                        // Convert encrypted data into a base64-encoded string.
                        // Return encrypted string.
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }

            public static string Decrypt(string encryptedText, SymmetricAlgorithm symmetricAlgorithm)
            {
                byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);

                CryptoStream cryptoStream = new CryptoStream(new MemoryStream(encryptedTextBytes),
                                                             symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Read);

                byte[] plainTextBytes = new byte[encryptedTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                cryptoStream.Close();

                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
        }
    }
}
