using BusinessObject;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace DUVAS
{
    public class InsiderTrading
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InsiderTradingId { get; set; }

        public int Remitter { get; set; }
        public User? Remitters { get; set; }

        public int Receiver { get; set; }
        public User? Receivers { get; set; }

        public decimal Money { get; set; }

        public string Note { get; set; }

        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public int? PriorityPackageRoomId { get; set; }
        public PriorityPackageRoom? PriorityPackageRoom { get; set; }

        public int Status { get; set; }
        // 1: Completed
        // 2: Pending
        // 3: Cancelled
        public string? Type { get; set; }
        // MuaGoi
        // ThanhToanHangThang
        // ThuePhongLanDau
        // 

        public DateTime CreatedDate { get; set; }

        public int HoldUntil { get; set; }

    }
}