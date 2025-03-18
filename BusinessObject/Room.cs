using BusinessObject;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DUVAS
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int? BuildingId { get; set; }
        public Building? Building { get; set; }
        public int CategoryRoomId { get; set; }
        public CategoryRoom? CategoryRoom { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string LocationDetail { get; set; }
        public double Acreage { get; set; }
        public string? Furniture { get; set; }

        public int NumberOfBathroom { get; set; }
        public int NumberOfBedroom { get; set; }
        public bool? Garret { get; set; }

        public decimal Price { get; set; }
        public decimal? Deposit { get; set; }

        public string Image { get; set; }

        public string? Note { get; set; }
        public int? status { get; set; }
        // 1: Đang trống
        // 2: Pending
        // 3: Đang được thuê
        public int? IsPermission { get; set; }

        //0: Lock
        //1: Bình thường
        public int? reputation { get; set; }
        //0: Không có tích xanh
        //1: Có tích xanh

        public virtual ICollection<RoomLicense>? RoomLicenses { get; set; }
        public virtual ICollection<SavedPost>? SavedPosts { get; set; }
        public virtual ICollection<RentalList>? RentalLists { get; set; }
        public virtual ICollection<PriorityPackageRoom>? PriorityPackageRooms { get; set; }
    }
}