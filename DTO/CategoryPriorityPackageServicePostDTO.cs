using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CategoryPriorityPackageServicePostDTO
    {
        [Key]
        public int CategoryPriorityPackageServicePostId { get; set; }

        [Required]
        public int CategoryPriorityPackageServicePostValue { get; set; }
        public int Status { get; set; }

        public decimal Price { get; set; }
    }
}