using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AuthorizationContractDTO
    {
        [Key]
        public int Id { get; set; }
        public string ContractNumber { get; set; }
        //public DateTime Date { get; set; }
        public int PartyAId { get; set; }
        public int PartyBId { get; set; }
        public string PdfUrl { get; set; }
        public string RoomList { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public int status { get; set; }
    }
}
