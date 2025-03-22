using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CategoryPriorityPackageServicePost
    {
        [Key]
        public int CategoryPriorityPackageServicePostId { get; set; }

        [Required]
        public int CategoryPriorityPackageServicePostValue { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<PriorityPackageServicePost>? PriorityPackageServicePosts { get; set; }
    }
}