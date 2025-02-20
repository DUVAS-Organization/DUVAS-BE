using System.ComponentModel.DataAnnotations;

namespace DUVAS;

public class BankAccounts
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? AccountNumber { get; set; }
    [Required]
    public string? BankName { get; set; }
    
    public User User { get; set; }
    public int UserId { get; set; }

}