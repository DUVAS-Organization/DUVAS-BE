using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class ServiceLicense
    {
        [Key]
        public int ServiceLicenseId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public string AnhCCCDMatTruoc { get; set; }
        public string AnhCCCDMatSau { get; set; }
        public int CCCD { get; set; }
        public string? GiayPhepKinhDoanh { get; set; }
        public string? GiayPhepChuyenMon { get; set; }
    }
}
