using BusinessObject;
using System;

namespace DTO
{
    public class PriorityPackageServicePostDTO
    {
        public int PriorityPackageServicePostId { get; set; }
        public int UserId { get; set; }
        public int ServicePostId { get; set; }
        public int CategoryPriorityPackageServicePostId { get; set; }
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public PriorityPackageServicePostDTO() { }  // Constructor không tham số

        public PriorityPackageServicePostDTO(PriorityPackageServicePost package)
        {
            PriorityPackageServicePostId = package.PriorityPackageServicePostId;
            UserId = package.UserId;
            ServicePostId = package.ServicePostId;
            CategoryPriorityPackageServicePostId = package.CategoryPriorityPackageServicePostId;
            StartDate = package.StartDate.ToString("HH:mm - dd/MM/yyyy");
            EndDate = package.EndDate.ToString("HH:mm - dd/MM/yyyy");
            Price = package.Price;
        }
    }
}
