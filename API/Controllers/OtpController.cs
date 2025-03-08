using API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;

namespace API.Controllers;

public class OtpController : ControllerBase
{
    
    private readonly ILogger<OtpController> _logger;
    private readonly OtpService _otpService;
    private readonly EmailService _emailService;
    private readonly IUserRepository _userRepository;

    public OtpController(ILogger<OtpController> logger, OtpService otpService, EmailService emailService, IUserRepository userRepository)
    {
        _logger = logger;
        _otpService = otpService;
        _emailService = emailService;
        _userRepository = userRepository;
    }
    
    // public OtpController(ILogger<OtpController> logger, OtpService otpService, EmailService emailService)
    // {
    //     _logger = logger;
    //     _otpService = otpService;
    //     _emailService = emailService;
    // }

    [HttpGet("otp")]
    [Authorize(Policy = "User")]
    public Task<IActionResult> GetOtp()
    { 
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim == null)
        {
            return Task.FromResult<IActionResult>(BadRequest(new {Message = "Unauthorized!"}));
        }
        int.TryParse(userIdClaim.Value, out int userId);
        var email = _userRepository.GetUserByIdAsync(userId).GetAwaiter().GetResult().Gmail;
        if (email == null)
        {
            return Task.FromResult<IActionResult>(StatusCode(500, new {Message= "Server Error."}));
        }
        var otp = _otpService.GenerateOtp(email);
        _emailService.SendEmail(email, "OTP", "Your OTP: " + otp);
        return Task.FromResult<IActionResult>(Ok(new {Message = "Please check your email to get an otp."}));
    }
}