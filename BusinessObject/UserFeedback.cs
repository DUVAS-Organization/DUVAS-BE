using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUVAS
{
    public class UserFeedback
    {
        [Key]
        public int UserFeedbackId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        // Thêm liên kết với Room
        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public string Comment { get; set; }

        [Range(1, 5)]
        public double Star { get; set; }
        public string Image { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
