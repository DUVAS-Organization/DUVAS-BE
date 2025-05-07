using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Service
{
    public class EncryptionService
    {
        private readonly string _key;

        public EncryptionService(IConfiguration configuration)
        {
            _key = configuration["EncryptionSettings:Key"]
                ?? throw new ArgumentNullException("EncryptionSettings:Key is missing in configuration.");

            // Đảm bảo khóa có độ dài 32 byte (cho AES-256)
            if (_key.Length < 32)
            {
                _key = _key.PadRight(32, '0'); // Padding với '0' nếu cần (có thể thay bằng logic an toàn hơn)
            }
            _key = _key.Substring(0, 32); // Cắt để đảm bảo đúng 32 byte
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_key);
                aes.GenerateIV(); // Tạo IV ngẫu nhiên cho mỗi lần mã hóa
                byte[] iv = aes.IV;

                using (var ms = new MemoryStream())
                {
                    // Lưu IV vào đầu dữ liệu mã hóa
                    ms.Write(iv, 0, iv.Length);

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    // Trả về dữ liệu mã hóa (IV + ciphertext) dưới dạng Base64
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                byte[] buffer = Convert.FromBase64String(cipherText);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(_key);

                    // Trích xuất IV từ đầu dữ liệu
                    byte[] iv = new byte[16];
                    Array.Copy(buffer, 0, iv, 0, iv.Length);
                    aes.IV = iv;

                    using (var ms = new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length))
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Giải mã thất bại: " + ex.Message, ex);
            }
        }
    }
}