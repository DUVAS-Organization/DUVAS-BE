using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUVAS
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime RentalDateTimeStart { get; set; } = DateTime.Now;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime RentalDateTimeEnd { get; set; }  = DateTime.Now;



        public string ContractFile { get; set; }

        public int status { get; set; }
        // 1: Chưa bị hủy hay hết hạn
        // 2: Đã bị hủy hoặc hét hạn
        // 3: pending

        public virtual ICollection<RentalList>? RentalLists { get; set; }
    }
}