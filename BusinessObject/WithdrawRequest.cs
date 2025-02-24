using System.ComponentModel.DataAnnotations;
using BusinessObject.Enums;

namespace DUVAS;
public class WithdrawRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
    public decimal Amount { get; set; }
    [MaxLength(50)]
    public required string BankCode { get; set; }
    [MaxLength(50)]
    public required string AccountNumber { get; set; }
    public WithdrawRequestStatus Status { get; set; }
    [MaxLength(500)]
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int TransactionId { get; set; }
    public Transaction? Transaction { get; set; }
}