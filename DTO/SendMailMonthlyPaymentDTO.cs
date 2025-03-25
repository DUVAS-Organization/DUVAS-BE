using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SendMailMonthlyPaymentDTO
    {
        public int userID { get; set; }
        public string userEmail { get; set; }
        public string userName { get; set; }
        public string roomName { get; set; }
        public string address { get; set; }
        public decimal price { get; set; }
        public decimal deposit { get; set; }
        public decimal khac { get; set; }
        public DateTime ngayBatDau { get; set; }
        public DateTime ngayKetThuc { get; set; }
    }
}