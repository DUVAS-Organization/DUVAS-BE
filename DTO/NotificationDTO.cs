using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string CreatedDate { get; set; }
    }
}