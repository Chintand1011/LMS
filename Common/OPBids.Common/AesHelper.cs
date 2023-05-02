using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Common
{
    public class AesHelper
    {
        private byte[] AesKey { get; set; }
        private byte[] AesIV { get; set; }

        public AesHelper(string key, string IV)
        {
            AesKey = Encoding.UTF8.GetBytes(key.PadLeft(32));
            AesIV = Encoding.UTF8.GetBytes(IV.PadLeft(16));
        }

        public string Encrypt(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            string encrypted = null;

            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(AesKey, AesIV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            return encrypted;
        }

        public string Decrypt(string cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(AesKey, AesIV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(cipherText)))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }


    }
}
