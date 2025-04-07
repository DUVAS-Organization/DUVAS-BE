namespace DTO;

public class ResetPasswordDTO
{
    public required string Otp { get; set; }
    public required string Password { get; set; }
    public required string RePassword { get; set; }
}