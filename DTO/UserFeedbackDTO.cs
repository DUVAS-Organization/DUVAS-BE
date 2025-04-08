using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserFeedbackDTO
    {
        [Key]
        public int UserFeedbackId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public double Star { get; set; }
        public string? Image { get; set; }
        public int? RoomId { get; set; }
        public int? ContractId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}