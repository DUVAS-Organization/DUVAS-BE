
using BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO
{
    public class TransactionAdminDTO
    {
        public string UserName { get; set; }
        public string Gmail { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? When { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}