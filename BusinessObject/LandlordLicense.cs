using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class LandlordLicense
    {
        [Key]
        public int LandlordLicenseId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public string? OwnerLicense1 { get; set; }
        public string? OwnerLicense2 { get; set; }
        public string? OwnerLicense3 { get; set; }
    }
}
