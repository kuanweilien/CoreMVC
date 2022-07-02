using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
namespace CoreMvc.Api
{
    public class Security
    {
        public static string PasswordEncrypt(string password, string key )
        {
            byte[] keyBytes = SHA256Encrypt(key);
            byte[] ivBytes = MD5Encrypt(key);

            return Convert.ToBase64String(EncryptStringToBytes_Aes(password, keyBytes, ivBytes));
        }
        //AES256
        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        //output 256 bits for Key
        private static byte[] SHA256Encrypt(string text)
        {
            var bytes = System.Text.Encoding.Default.GetBytes(text);
            var SHA256 = new SHA256CryptoServiceProvider();
            return SHA256.ComputeHash(bytes);
        }
        //output 128 bits for IV
        private static byte[] MD5Encrypt(string text)
        {
            byte[] bytes = Encoding.Default.GetBytes(text);
            var md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(bytes);
        }
    }
}
