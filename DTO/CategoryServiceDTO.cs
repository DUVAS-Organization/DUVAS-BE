using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CategoryServiceDTO
    {
        [Key]
        public int CategoryServiceId { get; set; }

        [Required]
        public string CategoryServiceName { get; set; }
    }
}
