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

        public int BuildingId { get; set; }        
        public string Title { get; set; }
        public string Description { get; set; }
        public string LocationDetail { get; set; }
        public double Acreage { get; set; }
        public string Furniture { get; set; }
        public int NumberOfBathroom { get; set; }
        public int NumberOfBedroom { get; set; }
        public bool Garret { get; set; }
        public decimal Price { get; set; }
        public string RoomCategory { get; set; }
        public string Image { get; set; }
        public string Note { get; set; }

        //public Building? Building { get; set; }
        //public bool IsPermission { get; set; }
    }
}
