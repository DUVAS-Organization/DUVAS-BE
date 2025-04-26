using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ContractRequestDTO
    {
        public int? ContractId { get; set; }
        // "yyyy-MM-dd
        public String RentalDateTimeStart { get; set; }
        public String RentalDateTimeEnd { get; set; }
        public string ContractFile { get; set; }
        public int Status { get; set; } = 1;
        public decimal? Deposit { get; set; }
        public decimal? Price { get; set; }
        public required int RoomId { get; set; }
        public int RenterID { get; set; }

    }
}