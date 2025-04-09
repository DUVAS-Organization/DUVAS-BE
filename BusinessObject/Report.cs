using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public string ReportContent { get; set; }
        public string Image { get; set; }
        public int? Status { get; set; }
        public string? Feedback {  get; set; }
    }
}
