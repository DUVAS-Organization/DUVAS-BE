using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class LandlordLicense
    {
        [Key]
        public int LandlordLicenseId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }


        public string AnhCCCDMatTruoc{ get; set; }
        public string AnhCCCDMatSau { get; set; }
        public int CCCD { get; set; }
        public string? GiayPhepKinhDoanh {  get; set; }
    }
}