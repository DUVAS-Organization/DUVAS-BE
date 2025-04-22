using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ContractDTO
    {
        public int ContractId { get; set; }
        public DateTime RentalDateTimeStart { get; set; }
        public DateTime RentalDateTimeEnd { get; set; }
        public decimal? DownPayment { get; set; }
        public string ContractFile { get; set; }
        public int Status { get; set; }
        public decimal? Deposit { get; set; }
        public decimal? Price { get; set; }
        public required int RoomId { get; set; }

    }
}