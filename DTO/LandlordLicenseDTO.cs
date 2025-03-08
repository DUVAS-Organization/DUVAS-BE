using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LandlordLicenseDTO
    {
        [Key]
        public int LandlordLicenseId { get; set; }

        public int UserId { get; set; }
        //public User? User { get; set; }

        public string AnhCCCDMatTruoc { get; set; }
        public string AnhCCCDMatSau { get; set; }
        public int CCCD { get; set; }
        public string? GiayPhepKinhDoanh { get; set; }
    }
}