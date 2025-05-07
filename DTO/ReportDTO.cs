using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ReportDTO
    {
        [Key]
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public int? RoomId { get; set; }
        public string RoomTitle { get; set; }//moi
        public string ReportContent { get; set; }
        public string Image { get; set; }
        public int? Status { get; set; }
        public string Feedback { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
