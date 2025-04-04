using DUVAS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RentalListDTO
    {
        [Key]
        public int RentalId { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public int? ContractId { get; set; }
        public Contract? Contract { get; set; }

        public int RenterID { get; set; }
        public string RenterName { get; set; }
        public string RenterEmail { get; set; }
        public string RenterPhone { get; set; }

        public DateTime RentDate { get; set; }
        public int MonthForRent { get; set; }
        public DateTime CreatedDate { get; set; }

        public int RentalStatus { get; set; }
        public int? ContractStatus { get; set; }

    }
}