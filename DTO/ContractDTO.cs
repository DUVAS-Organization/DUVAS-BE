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
        [Key]
        public int ContractId { get; set; }
        public DateTime RentalDateTimeStart { get; set; }
        public DateTime RentalDateTimeEnd { get; set; }
        public string ContractFile { get; set; }
    }
}
