using System.ComponentModel.DataAnnotations;

namespace DTO;

public class BankAccountUpdateDto
{
    [Required]
    public int BankAccountId { get; set; }
    [Required]
    public bool Active { get; set; }
    public required string Otp { get; set; }
}