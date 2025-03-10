using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SendMailDTO
    {        
        public  int UserIdLandlord  { get; set; }
        public  int RoomId  { get; set; }
        public string RenterName  { get; set; }
    }
}
