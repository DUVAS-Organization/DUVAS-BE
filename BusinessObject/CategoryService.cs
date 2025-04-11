using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class CategoryService
    {
        [Key]
        public int CategoryServiceId { get; set; }

        [Required]
        public string CategoryServiceName { get; set; }
        public int? Status { get; set; }
        public virtual ICollection<ServicePost>? ServicePosts { get; set; }
    }
}
