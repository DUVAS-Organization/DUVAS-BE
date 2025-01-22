using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MessageDTO
    {
        [Key]
        public int MessageId { get; set; }
        public string Content { get; set; }
    }
}
