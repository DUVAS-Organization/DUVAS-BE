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
        [MaxLength(12)]
        public String CCCD { get; set; }
        public string Name { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string? GiayPhepKinhDoanh { get; set; }
        public string? GiayPhepChuyenMon { get; set; }
        public int? Status { get; set; }
        //0: mặc định
        //1: đồng ý
        //2: Từ chối
        //public User? User { get; set; }
    }
}