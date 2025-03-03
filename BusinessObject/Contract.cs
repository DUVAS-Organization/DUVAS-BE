using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }

        public DateTime RentalDateTimeStart { get; set; }
        public DateTime RentalDateTimeEnd { get; set; }



        public string ContractFile { get; set; }

        public int status { get; set; }
        // 1: Chưa bị hủy hay hết hạn
        // 2: Đã bị hủy hoặc hét hạn

        public virtual ICollection<RentalList>? RentalLists { get; set; }
    }
}
