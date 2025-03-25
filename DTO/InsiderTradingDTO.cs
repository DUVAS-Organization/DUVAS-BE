using DUVAS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class InsiderTradingDTO
    {
        public int InsiderTradingId { get; set; }

        public int Remitter { get; set; }
        //public User? Remitters { get; set; }

        public int Receiver { get; set; }
        //public User? Receivers { get; set; }

        public decimal Money { get; set; }

        public string Note { get; set; }

        public int Status { get; set; }
        // 1: 
        // 2: 
        public string? Type { get; set; }

        public DateTime CreatedDate { get; set; }

        public int HoldUntil { get; set; }

    }
}