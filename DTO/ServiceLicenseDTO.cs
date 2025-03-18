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
        public string AnhCCCDMatTruoc { get; set; }
        public string AnhCCCDMatSau { get; set; }
        public int CCCD { get; set; }
        public string? GiayPhepKinhDoanh { get; set; }
        public string? GiayPhepChuyenMon { get; set; }
        //public User? User { get; set; }
    }
}
