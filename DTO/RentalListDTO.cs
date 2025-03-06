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
        //public Room? Room { get; set; }

        public int? ContractId { get; set; }
        //public Contract? Contract { get; set; }

        public int RenterID { get; set; }
        //public User? User { get; set; }

        public DateTime RentDate { get; set; }
        public int MonthForRent { get; set; }

        public DateTime CreatedDate { get; set; }

        public int RentalStatus { get; set; } = 0;

    }
}
