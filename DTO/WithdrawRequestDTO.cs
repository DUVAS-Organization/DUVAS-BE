
namespace DTO;

public class WithdrawRequestDTO
{
    public required decimal Amount { get; set; }
    public required string BankCode { get; set; }
    public required string AccountNumber { get; set; }
}