using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class UserFeedback
    {
        [Key]
        public int UserFeedbackId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public string Comment { get; set; }
        public int Star { get; set; }
        public string Image { get; set; }
    }
}
