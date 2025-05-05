using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DUVAS
{
    public static class EncryptionHelper
    {
        private static byte[] _aesKey;
        private static readonly object _lock = new object();
        private static bool _initialized = false;

        public static byte[] AesKey
        {
            get
            {
                if (!_initialized)
                {
                    throw new InvalidOperationException("EncryptionHelper not initialized. Call Initialize first.");
                }
                return _aesKey;
            }
        }

        public static void Initialize(IConfiguration configuration)
        {
            string base64Key = configuration["EncryptionSettings:AesKeyBase64"];
            if (string.IsNullOrEmpty(base64Key))
            {
                throw new InvalidOperationException("AES key not found in configuration.");
            }

            lock (_lock)
            {
                if (!_initialized)
                {
                    _aesKey = Convert.FromBase64String(base64Key);
                    _initialized = true;
                }
            }
        }

        public static (byte[] EncryptedData, byte[] IV) Encrypt(decimal value)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = AesKey;
                aes.GenerateIV();

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] data = Encoding.UTF8.GetBytes(value.ToString());
                        cs.Write(data, 0, data.Length);
                    }
                    return (ms.ToArray(), aes.IV);
                }
            }
        }

        public static decimal Decrypt(byte[] encryptedData, byte[] iv)
        {
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = AesKey;
                    aes.IV = iv;

                    using (var ms = new MemoryStream(encryptedData))
                    {
                        using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (var sr = new StreamReader(cs))
                        {
                            string decrypted = sr.ReadToEnd();
                            return decimal.Parse(decrypted);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CryptographicException($"Error decrypting data: {ex.Message}", ex);
            }
        }

        // New method for frontend display
        public static string GetDisplayValue(byte[] encryptedData, byte[] iv)
        {
            try
            {
                decimal value = Decrypt(encryptedData, iv);
                return value.ToString("N0"); // Format with thousands separator
            }
            catch
            {
                return "0"; // Return default value if decryption fails
            }
        }
    }
}