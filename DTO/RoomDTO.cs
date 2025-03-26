using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RoomDTO
    {
        [Key]
        public int RoomId { get; set; }
        public int? UserId { get; set; }
        public UserDTO? User { get; set; }
        public string? Name { get; set; }

        public string? UserName { get; set; }
        public int? BuildingId { get; set; }
        public string? BuildingName { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string LocationDetail { get; set; }
        public double Acreage { get; set; }
        public string Furniture { get; set; }
        public int NumberOfBathroom { get; set; }
        public int NumberOfBedroom { get; set; }
        public bool? Garret { get; set; }
        public decimal Price { get; set; }
        public int CategoryRoomId { get; set; }
        public string? CategoryName { get; set; }
        public string Image { get; set; }
        public string? Note { get; set; }
        public int? RentalId { get; set; }

        public int? IsPermission { get; set; }

        public decimal? Deposit { get; set; }
        public decimal? Dien { get; set; }
        public decimal? Nuoc { get; set; }
        public decimal? Internet { get; set; }
        public decimal? Rac { get; set; }
        public decimal? GuiXe { get; set; }
        public decimal? QuanLy { get; set; }
        public decimal? ChiPhiKhac { get; set; }
        public int? status { get; set; }
        public int? reputation { get; set; }
        public List<RentalListDTO>? RentalLists { get; set; }

        //public Building? Building { get; set; }
        //public bool IsPermission { get; set; }
    }
}