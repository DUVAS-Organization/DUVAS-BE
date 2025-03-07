using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace DUVAS
{
    public class InsiderTrading
    {
        [Key]
        public int InsiderTradingId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public decimal Money { get; set; }

        public string Note { get; set; }

        public int Status { get; set; }
        // 1: 
        // 2: 

        public DateTime CreatedDate { get; set; }

    }
}