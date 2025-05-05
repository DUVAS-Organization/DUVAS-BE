using BusinessObject;
using System;

namespace DTO
{
    public class PriorityPackageRoomDTO
    {
        public int PriorityPackageRoomId { get; set; }
        public int UserId { get; set; }
        public int? RoomId { get; set; }
        public int CategoryPriorityPackageRoomId { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }

        public PriorityPackageRoomDTO() { }  // Constructor không tham số

        public PriorityPackageRoomDTO(PriorityPackageRoom package)
        {
            PriorityPackageRoomId = package.PriorityPackageRoomId;
            UserId = package.UserId;
            RoomId = package.RoomId;
            CategoryPriorityPackageRoomId = package.CategoryPriorityPackageRoomId;
            StartDate = package.StartDate;
            EndDate = package.EndDate;
            Price = package.Price;
        }
    }
}