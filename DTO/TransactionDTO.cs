using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TransactionDTO
    {
        [Key]
        public int TransactionId { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public decimal Money { get; set; }
        public string TransactionType { get; set; }
        public int RoomId { get; set; }      
        public int ServicePostId { get; set; }      
        public int IDThue { get; set; }

        //public ServicePost? ServicePost { get; set; }
        //public Room? Room { get; set; }
        //public User? User { get; set; }
    }
}
