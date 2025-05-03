using BusinessObject;
using System;

namespace DTO
{
    public class PriorityPackageServicePostDTO
    {
        public int PriorityPackageServicePostId { get; set; }
        public int UserId { get; set; }
        public int? ServicePostId { get; set; }
        public int CategoryPriorityPackageServicePostId { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }

        public PriorityPackageServicePostDTO() { }  // Constructor không tham số

        public PriorityPackageServicePostDTO(PriorityPackageServicePost package)
        {
            PriorityPackageServicePostId = package.PriorityPackageServicePostId;
            UserId = package.UserId;
            ServicePostId = package.ServicePostId;
            CategoryPriorityPackageServicePostId = package.CategoryPriorityPackageServicePostId;
            StartDate = package.StartDate;
            EndDate = package.EndDate;
            Price = package.Price;
        }
    }
}