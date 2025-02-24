using System.ComponentModel.DataAnnotations;

namespace DTO;

public class DepositRequest
{
    public required int Amount { get; set; }
}