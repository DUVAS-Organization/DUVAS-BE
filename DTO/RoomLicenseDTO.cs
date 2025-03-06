using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RoomLicenseDTO
    {
        [Key]
        public int RoomLicenseId { get; set; }
        public int RoomId { get; set; }
        public string? BienBanPCCC { get; set; }

        //public Room? Room { get; set; }
    }
}
