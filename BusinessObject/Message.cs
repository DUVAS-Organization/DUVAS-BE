using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DUVAS
{
    [Table("Message")]
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        [ForeignKey("SenderId")]
        public int UserSendID { get; set; }
        public User? UserSend { get; set; }
        [ForeignKey("ReceiverId")]
        public int UserGetID { get; set; }
        public User? UserGet { get; set; }

        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime DateTime { get; set; }
        public int Status { get; set; }

        [JsonConstructor]
        public Message() { }


    }
}