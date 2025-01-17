namespace DTO;

public class RegisterDto
{
    public required string Otp { get; set; }
    public required string UserName { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required string RePassword { get; set; }
    public required string Address { get; set; }
    public required string Sex { get; set; }
}