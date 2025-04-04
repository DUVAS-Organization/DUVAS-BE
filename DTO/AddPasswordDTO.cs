using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AddPasswordDTO
    {
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}