using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class CategoryRoom
    {
        [Key]
        public int CategoryRoomId { get; set; }

        [Required]
        public string CategoryName { get; set; }
        public int? Status { get; set; } = 1;
        public virtual ICollection<Room>? Rooms { get; set; }
    }
}
