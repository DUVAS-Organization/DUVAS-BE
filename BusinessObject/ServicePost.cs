using BusinessObject;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class ServicePost
    {
        [Key]
        public int ServicePostId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? IsPermission { get; set; }
        //0: Lock
        //1: Bình thường

        // Mối quan hệ với CategoryService
        public int CategoryServiceId { get; set; }
        public CategoryService? CategoryService { get; set; }

        // Mối quan hệ với ServiceFeedback
        public virtual ICollection<ServiceFeedback>? ServiceFeedbacks { get; set; }
        public virtual ICollection<SavedPost>? SavedPosts { get; set; }
        public virtual ICollection<RentalServiceList>? RentalServiceLists { get; set; }
        public virtual ICollection<PriorityPackageServicePost>? PriorityPackageServicePosts { get; set; }

    }
}
