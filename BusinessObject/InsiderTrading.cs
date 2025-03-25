using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace DUVAS
{
    public class InsiderTrading
    {
        [Key]
        public int InsiderTradingId { get; set; }

        public int Remitter { get; set; }
        public User? Remitters { get; set; }

        public int Receiver { get; set; }
        public User? Receivers { get; set; }

        public decimal Money { get; set; }

        public string Note { get; set; }

        public int Status { get; set; }
        // 1: Completed
        // 2: Pending
        // 3: Cancelled
        public string? Type { get; set; }
        // MuaGoi+
        // MuaGoi-
        // ThanhToanHangThang+
        // ThanhToanHangThang-
        // ThuePhongLanDau+
        // ThuePhongLanDau-
        // RutTien


        public DateTime CreatedDate { get; set; }

        public int HoldUntil { get; set; }

    }
}