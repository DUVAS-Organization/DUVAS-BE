using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CategoryRoomDTO
    {
        [Key]
        public int CategoryRoomId { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
