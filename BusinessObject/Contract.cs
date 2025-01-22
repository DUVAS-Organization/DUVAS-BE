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

        public virtual ICollection<RentalList>? RentalLists { get; set; }
    }
}
