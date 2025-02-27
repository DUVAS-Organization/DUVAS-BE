using DUVAS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PriorityPackageRoom
    {
        [Key]
        public int PriorityPackageRoomId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public int CategoryPriorityPackageRoomId { get; set; }
        public CategoryPriorityPackageRoom? CategoryPriorityPackageRoom { get; set; }
        public DateTime StartDate {  get; set; }
        public DateTime EndDate {  get; set; }
        public decimal Price {  get; set; }
    }
}
