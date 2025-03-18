using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UpdateBalanceDTO
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}