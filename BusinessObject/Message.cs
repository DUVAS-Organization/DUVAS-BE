using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public int UserSendID { get; set; }
        public User? UserSend { get; set; }

        [Required]
        public int UserGetID { get; set; }
        public User? UserGet { get; set; }

        public string? Content { get; set; }
        public string? Image {  get; set; }
        public DateTime DateTime { get; set; }
        public int Status { get; set; }
    }
}
