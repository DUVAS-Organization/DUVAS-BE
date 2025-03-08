using System.ComponentModel.DataAnnotations;

namespace DTO;

public class WithdrawRequestDTO
{
    public required decimal Amount { get; set; }
    public required int BankAccountId { get; set; }
}