using DUVAS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CategoryPriorityPackageRoom
    {
        [Key]
        public int CategoryPriorityPackageRoomId { get; set; }

        [Required]
        public int CategoryPriorityPackageRoomValue { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; } = 1;

        public virtual ICollection<PriorityPackageRoom>? PriorityPackageRooms { get; set; }
    }
}