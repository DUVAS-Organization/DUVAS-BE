using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUVAS
{
    public class SavedPost
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }  // 🔥 ID của bài đăng

        [Required]
        public int UserId { get; set; }  // 🔥 ID của user đã lưu bài đăng
        public virtual Room? Room { get; set; }  // Liên kết với bảng Room
        public virtual User? User { get; set; }  // Liên kết với bảng User

        public DateTime? SavedAt { get; set; } = DateTime.Now;
    }
}
