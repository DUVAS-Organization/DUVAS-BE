using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace DUVAS
{
    public class RentalList
    {
        [Key]
        public int RentalId { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public int? ContractId { get; set; }
        public Contract? Contract { get; set; }

        public int RenterID { get; set; }
        public User? User { get; set; }

        public DateTime RentDate { get; set; }
        public int MonthForRent { get; set; }

        public DateTime CreatedDate { get; set; }

        public int RentalStatus { get; set; } = 0;
        // 1: Đang chờ chủ liên hệ và đồng ý
        // 2: Đã hủy yêu cầu thuê
    }
}