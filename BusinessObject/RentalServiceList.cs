using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace DUVAS
{
    public class RentalServiceList
    {
        [Key]
        public int RentalServiceId { get; set; }

        public DateTime CreationDateTime { get; set; }
        public DateTime RentalDateTime { get; set; }

        public int ServicePostID { get; set; }
        public ServicePost? ServicePost { get; set; }

        public int RenterID { get; set; }
        public User? User { get; set; }

        public int RentalServiceStatus { get; set; } = 0;
    }
}
