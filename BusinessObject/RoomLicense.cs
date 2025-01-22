using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class RoomLicense
    {
        [Key]
        public int RoomLicenseId { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public string RoomLicense1 { get; set; }
        public string RoomLicense2 { get; set; }
        public string RoomLicense3 { get; set; }

    }
}
