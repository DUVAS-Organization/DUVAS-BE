using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ServiceLicenseDTO
    {
        [Key]
        public int ServiceLicenseId { get; set; }
        public int UserId { get; set; }
        public string ServiceLicense1 { get; set; }
        public string ServiceLicense2 { get; set; }
        //public User? User { get; set; }
    }
}
