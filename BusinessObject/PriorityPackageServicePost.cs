using DUVAS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PriorityPackageServicePost
    {
        [Key]
        public int PriorityPackageServicePostId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int ServicePostId { get; set; }
        public ServicePost? ServicePost { get; set; }
        public int CategoryPriorityPackageServicePostId { get; set; }
        public CategoryPriorityPackageServicePost? CategoryPriorityPackageServicePost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
    }
}