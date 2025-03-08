using System.ComponentModel.DataAnnotations;

namespace DTO;

public class BankAccountsDTO
{
    [Required]
    public string AccountNumber { get; set; }
    [Required]
    public string BankCode { get; set; }
    
}