namespace DTO;

public class ResetPassword
{
    public required string Otp { get; set; }
    public required string Password { get; set; }
    public required string RePassword { get; set; }
}