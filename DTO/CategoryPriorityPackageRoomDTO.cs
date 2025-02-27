using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CategoryPriorityPackageRoomDTO
    {
        [Key]
        public int CategoryPriorityPackageRoomId { get; set; }

        [Required]
        public int CategoryPriorityPackageRoomValue { get; set; }

        public decimal Price { get; set; }
    }
}
