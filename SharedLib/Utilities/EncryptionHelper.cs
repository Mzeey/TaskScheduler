using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Utilities
{
    public class EncryptionHelper
    {
        public static string Encrypt(string valueToEncrypt, string encryptionKey)
        {
            byte[] valueToEncryptBytes = Encoding.UTF8.GetBytes(valueToEncrypt);
            byte[] encryptionKeyBytes = Encoding.UTF8.GetBytes(encryptionKey);


            using (Aes aesAlgo = Aes.Create())
            {
                aesAlgo.Key = encryptionKeyBytes;
                aesAlgo.GenerateIV();

                ICryptoTransform encryptor = aesAlgo.CreateEncryptor(aesAlgo.Key, aesAlgo.IV);
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(valueToEncryptBytes, 0, valueToEncrypt.Length);
                        csEncrypt.FlushFinalBlock();

                        byte[] encryptedBytes = msEncrypt.ToArray();
                        byte[] encryptedValueBytes = new byte[aesAlgo.IV.Length + encryptedBytes.Length];
                        Array.Copy(aesAlgo.IV, encryptedValueBytes, aesAlgo.IV.Length);
                        Array.Copy(encryptedBytes, 0, encryptedValueBytes, aesAlgo.IV.Length, encryptedBytes.Length);

                        string encryptedValue = Convert.ToBase64String(encryptedValueBytes);
                        return encryptedValue;
                    }
                }
            }
        }

        public static string Decrypt(string valueToDecrypt, string decryptionKey)
        {
            byte[] encryptedValueBytes = Convert.FromBase64String(valueToDecrypt);
            byte[] decryptionKeyBytes = Encoding.UTF8.GetBytes(decryptionKey);
            using (Aes aesAlgo = Aes.Create())
            {
                aesAlgo.Key = decryptionKeyBytes;
                byte[] iv = new byte[aesAlgo.IV.Length];
                Array.Copy(encryptedValueBytes, iv, iv.Length);
                aesAlgo.IV = iv;

                ICryptoTransform decryptor = aesAlgo.CreateDecryptor(aesAlgo.Key, aesAlgo.IV);
                using (var msDecrypt = new System.IO.MemoryStream(encryptedValueBytes, iv.Length, encryptedValueBytes.Length - iv.Length))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            string decryptedValue = srDecrypt.ReadToEnd();
                            return decryptedValue;
                        }
                    }
                }
            }
        }
    }
}
