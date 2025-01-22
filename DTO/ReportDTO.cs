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
        //public User? User { get; set; }

        public int? RoomId { get; set; }
        //public Room? Room { get; set; }

        public int? ServicePostId { get; set; }
        //public ServicePost? ServicePost { get; set; }

        public int? TransactionId { get; set; }
        //public Transaction? Transaction { get; set; }

        public string ReportContent { get; set; }
        public string Image { get; set; }
    }
}
