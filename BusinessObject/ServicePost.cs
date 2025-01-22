using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class ServicePost
    {
        [Key]
        public int ServicePostId { get; set; }

        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        // Mối quan hệ với CategoryService
        public int CategoryServiceId { get; set; }
        public CategoryService? CategoryService { get; set; }

        // Mối quan hệ với ServiceFeedback
        public virtual ICollection<ServiceFeedback>? ServiceFeedbacks { get; set; }
    }
}
