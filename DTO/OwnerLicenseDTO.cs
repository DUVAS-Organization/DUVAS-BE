using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class OwnerLicenseDTO
    {
        [Key]
        public int OwnerLicenseId { get; set; }

        public int UserId { get; set; }
        //public User? User { get; set; }

        public string OwnerLicense1 { get; set; }
        public string OwnerLicense2 { get; set; }
        public string OwnerLicense3 { get; set; }
    }
}
