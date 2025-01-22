using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class ServiceFeedback
    {
        [Key]
        public int ServiceFeedbackId { get; set; }

        public int ServicePostId { get; set; }
        public ServicePost? ServicePost { get; set; }

        public string Comment { get; set; }
        public int Star { get; set; }
        public string Image { get; set; }
    }
}
