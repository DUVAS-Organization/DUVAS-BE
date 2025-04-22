using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public string Type { get; set; } // Loại thông báo: "CancelRegisterUpRole", "AcceptRegisterUpRole",........
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }

}