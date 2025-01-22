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
        public string RoomLicense1 { get; set; }
        public string RoomLicense2 { get; set; }
        public string RoomLicense3 { get; set; }

        //public Room? Room { get; set; }
    }
}
