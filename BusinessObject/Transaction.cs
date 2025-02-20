using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BusinessObject.Enums;

namespace DUVAS
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? CassoId { get; set; }
        public string? TId { get; set; }
        [MaxLength(500)]
        public string? Description {get; set;}
        [Required]
        public decimal Amount { get; set; }
        public decimal? CusumBalance { get; set; }
        public DateTime? When { get; set; }
        [MaxLength(100)]
        public string? BankSubAccID {get; set;}
        [MaxLength(100)]
        public string? SubAccID {get; set;}
        [MaxLength(100)]
        public string? BankName {get; set;}
        [MaxLength(100)]
        public string? bankAbbreviation {get; set;}
        [MaxLength(100)]
        public string? CorresponsiveName { get; set; }
        [MaxLength(100)]
        public string? CorresponsiveAccount { get; set; }
        [MaxLength(100)]
        public string? CorresponsiveBankId { get; set; }
        [MaxLength(100)]
        public string? CorresponsiveBankName { get; set; }
        [Required]
        public int UserId;
        
        [JsonIgnore]
        public User? User { get; set; }
        [Required]
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
