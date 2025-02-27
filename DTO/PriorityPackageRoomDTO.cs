using BusinessObject;
using System;

namespace DTO
{
    public class PriorityPackageRoomDTO
    {
        public int PriorityPackageRoomId { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public int CategoryPriorityPackageRoomId { get; set; }
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public PriorityPackageRoomDTO() { }  // Constructor không tham số

        public PriorityPackageRoomDTO(PriorityPackageRoom package)
        {
            PriorityPackageRoomId = package.PriorityPackageRoomId;
            UserId = package.UserId;
            RoomId = package.RoomId;
            CategoryPriorityPackageRoomId = package.CategoryPriorityPackageRoomId;
            StartDate = package.StartDate.ToString("HH:mm - dd/MM/yyyy");
            EndDate = package.EndDate.ToString("HH:mm - dd/MM/yyyy");
            Price = package.Price;
        }
    }
}
