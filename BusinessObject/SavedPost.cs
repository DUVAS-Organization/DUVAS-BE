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
        public int RoomId { get; set; } 

        [Required]
        public int UserId { get; set; }  
        public virtual Room? Room { get; set; } 
        public virtual User? User { get; set; }

        public DateTime? SavedAt { get; set; } = DateTime.Now;
    }
}
