using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace DUVAS
{
    public class RentalServiceList
    {
        [Key]
        public int RentalServiceId { get; set; }

        public DateTime CreationDateTime { get; set; } //Ngay gio tao
        public DateTime? AcceppDateTime { get; set; } //Ngay gio accepp
        public DateTime? RentalDateTime { get; set; } //Ngày giờ hẹn booking

        public int ServicePostID { get; set; }
        public ServicePost? ServicePost { get; set; }

        public int RenterID { get; set; }
        public User? User { get; set; }

        public int RentalServiceStatus { get; set; } = 0;
        // 1: tao yeu cau thue
        // 2: da chap nhan
        // 3: da hoan thanh
    }
}
