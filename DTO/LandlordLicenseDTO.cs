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
        [MaxLength(12)]
        public string CCCD { get; set; }
        public string Name { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string? GiayPhepKinhDoanh { get; set; }
        public int? Status { get; set; }
    }
    public class ExtractedDataDTO
    {
        public string AnhCCCDMatTruoc { get; set; }
        public string AnhCCCDMatSau { get; set; }
        [MaxLength(12)]
        public String CCCD { get; set; }
        public string Name { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
    }
    // DTO cho phản hồi từ FPT AI
    public class FPTAIResponseDTO
    {
        public List<FPTAIDataItem> Data { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class FPTAIDataItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Dob { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public string FrontImage { get; set; }
        public string BackImage { get; set; }
    }
}