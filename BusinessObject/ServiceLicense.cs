using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class ServiceLicense
    {
        [Key]
        public int ServiceLicenseId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public string ServiceLicense1 { get; set; }
        public string ServiceLicense2 { get; set; }
    }
}
