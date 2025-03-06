using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class RoomLicense
    {
        [Key]
        public int RoomLicenseId { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public string? BienBanPCCC { get; set; }


    }
}
