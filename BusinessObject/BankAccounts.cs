using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BusinessObject.Enums;

namespace DUVAS;

public class BankAccounts
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string? AccountNumber { get; set; }
    [Required]
    [MaxLength(50)]
    public string? BankCode { get; set; }
    public BankAccountStatus Status { get; set; }
    
    [JsonIgnore]
    [Required]
    public User? User { get; set; }
    public int? UserId { get; set; }

}