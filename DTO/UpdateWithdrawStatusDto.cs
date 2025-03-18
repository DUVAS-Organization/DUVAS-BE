using System.ComponentModel.DataAnnotations;

namespace DTO;

public class UpdateWithdrawStatusDto
{
    [Required]
    public String Status { get; set; } = string.Empty;
}