using System;
using System.Security.Cryptography;
using System.Text;

namespace BusinessObject.Service
{
    public class EncryptionService
    {
        private readonly string _key; // Chỉ có một trường _key duy nhất

        public EncryptionService(string key)
        {
            _key = key.PadRight(32).Substring(0, 32); // Đảm bảo khóa 32 byte cho AES-256
        }

        public string Encrypt(string plainText, string iv)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_key);
                aes.IV = Encoding.UTF8.GetBytes(iv.PadRight(16).Substring(0, 16)); // Đảm bảo IV 16 byte
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(encrypted);
            }
        }

        public string Decrypt(string cipherText, string iv)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_key);
                aes.IV = Encoding.UTF8.GetBytes(iv.PadRight(16).Substring(0, 16));
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(decrypted);
            }
        }
    }
}