using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ServiceFeedbackDTO
    {
        [Key]
        public int ServiceFeedbackId { get; set; }
        public int ServicePostId { get; set; }      
        public string Comment { get; set; }
        public int Star { get; set; }
        public string Image { get; set; }

        //public ServicePost? ServicePost { get; set; }
    }
}
