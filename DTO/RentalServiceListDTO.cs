using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RentalServiceListDTO
    {
        [Key]
        public int RentalServiceId { get; set; }

        public int ServicePostId { get; set; }
        //public Room? Room { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime RentalDateTime { get; set; }

        public int RenterServiceID { get; set; }
        //public User? User { get; set; }
    }
}
