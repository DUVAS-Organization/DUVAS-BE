using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace DUVAS
{
    public class RentalList
    {
        [Key]
        public int RentalId { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public int ContractId { get; set; }
        public Contract? Contract { get; set; }

        public int RenterID { get; set; }
        public User? User { get; set; }
    }
}
