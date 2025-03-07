using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserFeedbackDTO
    {
        [Key]
        public int UserFeedbackId { get; set; }
        public int UserId { get; set; }       
        public string Comment { get; set; }
        public int Star { get; set; }
        public string Image { get; set; }
        //public User? User { get; set; }
    }
}
