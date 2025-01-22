using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string Content { get; set; }
    }
}
