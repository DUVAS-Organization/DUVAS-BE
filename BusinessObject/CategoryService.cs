using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class CategoryService
    {
        [Key]
        public int CategoryServiceId { get; set; }

        [Required]
        public string CategoryServiceName { get; set; }
        public int Status { get; set; } = 1;

        public virtual ICollection<ServicePost>? ServicePosts { get; set; }
    }
}
