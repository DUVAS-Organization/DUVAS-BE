using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using BusinessObject.Service;

namespace DUVAS
{
    public class ServiceLicense
    {
        private readonly EncryptionService _encryptionService;

        public ServiceLicense()
        {
        }

        public ServiceLicense(EncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        [Key]
        public int ServiceLicenseId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        private string? _anhCCCDMatTruoc;
        private string? _anhCCCDMatSau;
        private string? _anhCCCDMatTruocIV;
        private string? _anhCCCDMatSauIV;

        [MaxLength(500)]
        public string? AnhCCCDMatTruoc
        {
            get => _encryptionService?.Decrypt(_anhCCCDMatTruoc, _anhCCCDMatTruocIV);
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _anhCCCDMatTruoc = null;
                    _anhCCCDMatTruocIV = null;
                }
                else if (_encryptionService != null)
                {
                    _anhCCCDMatTruocIV = GenerateIV();
                    _anhCCCDMatTruoc = _encryptionService.Encrypt(value, _anhCCCDMatTruocIV);
                }
            }
        }

        [MaxLength(500)]
        public string? AnhCCCDMatSau
        {
            get => _encryptionService?.Decrypt(_anhCCCDMatSau, _anhCCCDMatSauIV);
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _anhCCCDMatSau = null;
                    _anhCCCDMatSauIV = null;
                }
                else if (_encryptionService != null)
                {
                    _anhCCCDMatSauIV = GenerateIV();
                    _anhCCCDMatSau = _encryptionService.Encrypt(value, _anhCCCDMatSauIV);
                }
            }
        }

        [MaxLength(12)]
        public string? CCCD { get; set; }

        [MaxLength(16)]
        public string? AnhCCCDMatTruocIV { get; set; }

        [MaxLength(16)]
        public string? AnhCCCDMatSauIV { get; set; }

        public string? Name { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string? Sex { get; set; }
        public string? Address { get; set; }
        public string? GiayPhepKinhDoanh { get; set; }
        public string? GiayPhepChuyenMon { get; set; }
        public int? Status { get; set; } = 0;
        private static string GenerateIV()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] iv = new byte[16];
                rng.GetBytes(iv);
                return Convert.ToBase64String(iv).Substring(0, 16);
            }
        }
    }
}