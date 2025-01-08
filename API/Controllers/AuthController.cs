using System.Text.RegularExpressions;
using API.Service;
using DTO;
using DUVAS;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.IRepository;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly OtpService _otpService;
        private readonly IUserRepository _iuserRepository;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly TokenExchangeService _tokenExchangeService;
        private readonly TokenDictionaryService _tokenDictionaryService;
        public AuthController(EmailService emailService, OtpService otpService, IUserRepository iuserRepository, JwtService jwtService, IConfiguration configuration, TokenExchangeService tokenExchangeService, TokenDictionaryService tokenDictionaryService)
        {
            _emailService = emailService;
            _otpService = otpService;
            _iuserRepository = iuserRepository;
            _jwtService = jwtService;
            _configuration = configuration;
            _tokenExchangeService = tokenExchangeService;
            _tokenDictionaryService = tokenDictionaryService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _iuserRepository.GetUserByGmailOrPhoneAsync(loginDto.Username);
            if (user == null)
            {
                user = await _iuserRepository.GetUserByUsernameAsync(loginDto.Username);
            }

            if (user == null)
            {
                return BadRequest(new { Message = "Username or password is incorrect" });
            }

            if (BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return Ok(new { Message = _jwtService.GenerateToken(user) });
            }
            return BadRequest(new { Message = "Username or password is incorrect" });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var emailOrPhone = _otpService.CheckOtp(registerDto.Otp);
            if (emailOrPhone == null)
            {
                return BadRequest(new { Message = "Invalid or expired OTP." });
            }

            var checkUsername = await _iuserRepository.GetUserByUsernameAsync(registerDto.UserName);
            if (checkUsername != null)
            {
                return BadRequest(new { Message = "Username is already taken." });
            }
        
            if (registerDto.Password != registerDto.RePassword)
            {
                return BadRequest(new { Message = "Passwords do not match." });
            }

            if (!Regex.IsMatch(registerDto.Password, @"^(?=.*[A-Z]).{8,}$"))
            {
                return BadRequest(new { Message = "Password must have at least 8 character and 1 upper case letter." });
            }
            var user = new User(emailOrPhone, registerDto.UserName, registerDto.Name, BCrypt.Net.BCrypt.HashPassword(registerDto.Password), registerDto.Address, registerDto.Sex, "/default.png", 0, 1);
            try
            {
                await _iuserRepository.SaveUserAsync(user);
                _otpService.RemoveOtp(emailOrPhone);
                return Ok(new {Message= "Register successful."});
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return StatusCode(500, new {Message= "Register fail."});
            }
            
        }

        [HttpPost("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> Verify([FromBody] VerifyDto verifyDto)
        {
            var existingUser = await _iuserRepository.GetUserByGmailOrPhoneAsync(verifyDto.EmailOrPhone);
            if (existingUser == null)
            {
                var otp = _otpService.GenerateOtp(verifyDto.EmailOrPhone);
                _emailService.SendEmail(verifyDto.EmailOrPhone, "Verify your email address", "Use this otp to register your new account: " + otp);
                return Ok(new {Message = "please check your email to get an otp."});
            }
            return BadRequest(new {Message = "Email or phone already in use."});
        }

        [HttpGet("google")]
        public IActionResult Google()
        {
            return Redirect("https://accounts.google.com/o/oauth2/auth?client_id=" + _configuration["Google:ClientId"] + "&redirect_uri=" + _configuration["Google:RedirectUri"] + "&response_type=code&scope=openid%20profile%20email");
        }

        [HttpGet("login-google")]
        public async Task<IActionResult> LoginGoogle([FromQuery] String code)
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _configuration["Google:ClientId"],
                    ClientSecret = _configuration["Google:ClientSecret"]
                },
                Scopes = new[] { "openid", "profile", "email" }
            });
            var tokenResponse = await flow.ExchangeCodeForTokenAsync(
                userId: "me",
                code: code,
                redirectUri: _configuration["Google:RedirectUri"],
                taskCancellationToken: CancellationToken.None
            );
            var payload = GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken).Result;
            var email = payload.Email;
            var name = payload.Name;
            var avatar = payload.Picture;
            var user = await _iuserRepository.GetUserByGmailOrPhoneAsync(email);
            Console.WriteLine(user == null ? "new user" : "existing user");
            if (user == null)
            {
                user = new User(email, name, email, avatar, 0);
                await _iuserRepository.SaveUserAsync(user);
            }

            if (user.Gmail == null)
            {
                return StatusCode(500, new {Message= "Server Error."});
            }
            String codeExchange = _tokenDictionaryService.GenerateCode(user.Gmail);
            return Redirect("http://localhost:3000?token=" + codeExchange);
        }

        [HttpGet("token-exchange")]
        public async Task<IActionResult> TokenExchange([FromQuery] String code)
        {
            try
            {
                string accessToken = _tokenExchangeService.HandleExchangeRequest(code);
                return Ok(new { Access_token = accessToken });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, new {Message= "Server Error."});
            }
        }

        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] String emailOrPhone)
        {
            var otp = _otpService.GenerateOtp(emailOrPhone);
            _emailService.SendEmail(emailOrPhone, "Reset your password", "Use this otp to reset your password: " + otp);
            return Ok(new {Message = "Please check your email to get an otp."});
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            var emailOrPhone = _otpService.CheckOtp(resetPassword.Otp);
            if (emailOrPhone == null)
            {
                return BadRequest(new { Message = "Invalid or expired OTP." });
            }

            if (resetPassword.Password != resetPassword.RePassword)
            {
                return BadRequest(new {Message= "Passwords don't match."});
            }
            if (!Regex.IsMatch(resetPassword.Password, @"^(?=.*[A-Z]).{8,}$"))
            {
                return BadRequest(new { Message = "Password must have at least 8 character and 1 upper case letter." });
            }
            if (_iuserRepository.UpdatePasswordAsync(emailOrPhone, BCrypt.Net.BCrypt.HashPassword(resetPassword.Password)).Result)
            {
                _otpService.RemoveOtp(emailOrPhone);
                return Ok(new {Message = "Password updated successfully."});
            }
            return StatusCode(500, new {Message= "Update password failed."});
        }

        [HttpGet("authenticated")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok(new {Message = "Authenticated endpoint"});
        }

        [HttpGet("user")]
        [Authorize(Policy = "User")]
        public IActionResult GetUser()
        {
            return Ok(new {Message = "Authenticated endpoint for User"});
        }
        
        [HttpGet("admin")]
        [Authorize(Policy = "Admin")]
        public IActionResult GetAdmin()
        {
            return Ok(new {Message = "Authenticated endpoint for Admin"});
        }
        
        [HttpGet("landlord")]
        [Authorize(Policy = "Landlord")]
        public IActionResult GetLandlord()
        {
            return Ok(new {Message = "Authenticated endpoint for Landlord"});
        }
        
        [HttpGet("service")]
        [Authorize(Policy = "Service")]
        public IActionResult GetService()
        {
            return Ok(new {Message = "Authenticated endpoint for Service"});
        }
    }
}
    