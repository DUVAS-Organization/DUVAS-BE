using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserDTO
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Gmail { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string ProfilePicture { get; set; }
        public decimal Money { get; set; }

        //public int? RoleAdmin { get; set; }
        //public int? RoleUser { get; set; }
        //public int? RoleLandlord { get; set; }
        //public int? RoleService { get; set; }
    }
}
