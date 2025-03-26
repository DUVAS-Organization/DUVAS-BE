using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DUVAS;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using DTO;
using System.Text.RegularExpressions;
using API.Service;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IServicePostRepository _servicePostRepository;
        private readonly IRentalListRepository _rentalListRepository;
        private readonly IRentalServiceListRepository _rentalServiceListRepository;
        private readonly IContractRepository _contractRepository;
        private readonly OtpService _otpService;
        private readonly EmailService _emailService;

        public UserProfileController(IUserRepository userRepository, IRoomRepository roomRepository,
            IServicePostRepository servicePostRepository, IRentalListRepository rentalListRepository, IContractRepository contractRepository,
            IRentalServiceListRepository rentalServiceListRepository,
            OtpService otpService, EmailService emailService)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _servicePostRepository = servicePostRepository;
            _rentalListRepository = rentalListRepository;
            _contractRepository = contractRepository;
            _rentalServiceListRepository = rentalServiceListRepository;
            _otpService = otpService;
            _emailService = emailService;
        }

        public class ChangePasswordModel
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        // Thêm endpoint GET để lấy thông tin người dùng theo userId
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = "User không tồn tại." });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Lỗi khi lấy thông tin người dùng.", Details = ex.Message });
            }
        }

        //[HttpPut("{id}/EditProfile")]
        //public async Task<IActionResult> EditProfile(int id, [FromBody] User updatedUser)
        //{
        //    if (id != updatedUser.UserId)
        //    {
        //        return BadRequest("ID không khớp.");
        //    }

        //    var user = await _userRepository.GetUserByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound("User không tồn tại.");
        //    }

        //    user.UserName = updatedUser.UserName ?? user.UserName;
        //    user.Name = updatedUser.Name ?? user.Name;
        //    //user.Gmail = updatedUser.Gmail ?? user.Gmail;
        //    user.Phone = updatedUser.Phone ?? user.Phone;
        //    user.Address = updatedUser.Address ?? user.Address;
        //    user.Sex = updatedUser.Sex ?? user.Sex;
        //    user.ProfilePicture = updatedUser.ProfilePicture ?? user.ProfilePicture;

        //    await _userRepository.UpdateUserAsync(user);

        //    return NoContent();
        //}
        [HttpPut("edit-profile/{id}")]
        public async Task<IActionResult> EditProfile(int id, [FromBody] EditProfileRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (request.UserName != null) user.UserName = request.UserName;
            if (request.Name != null) user.Name = request.Name;
            if (request.Phone != null) user.Phone = request.Phone;
            if (request.Address != null) user.Address = request.Address;
            if (request.Sex != null) user.Sex = request.Sex;
            if (request.ProfilePicture != null) user.ProfilePicture = request.ProfilePicture;

            await _userRepository.UpdateUserAsync(user);
            return Ok("Profile updated successfully");
        }
        // API: View room rental history
        [HttpGet("{userId}/RoomRentalHistory")]
        public async Task<IActionResult> GetRoomRentalHistory(int userId)
        {
            try
            {
                // Lấy danh sách RentalList theo UserId
                var rentalLists = await _rentalListRepository.GetRentalListsAsync();
                var userHistory = rentalLists.Where(r => r.RenterID == userId).ToList();

                if (!userHistory.Any())
                {
                    return NotFound("Không có lịch sử thuê phòng.");
                }

                return Ok(userHistory);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi lấy lịch sử thuê phòng.");
            }
        }

        // API: View service rental history
        [HttpGet("{userId}/ServiceRentalHistory")]
        public async Task<IActionResult> ServiceRentalHistory(int userId)
        {
            try
            {
                // Lấy danh sách RentalServiceLists theo UserId
                var rentalServiceLists = await _rentalServiceListRepository.GetRentalServiceListsAsync();
                var userServiceHistory = rentalServiceLists
                    .Where(r => r.RenterServiceID == userId)
                    .ToList();

                if (!userServiceHistory.Any())
                {
                    return NotFound("Không có lịch sử sử dụng dịch vụ.");
                }

                return Ok(userServiceHistory);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi lấy lịch sử sử dụng dịch vụ.");
            }
        }

        [HttpPost("changePassword")]
        [Authorize] // Chỉ cho phép người dùng đã đăng nhập
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            // Lấy thông tin người dùng từ token JWT
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Unauthorized" });
            }

            // Lấy thông tin người dùng từ repository
            var user = await _userRepository.GetUserByIdAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Kiểm tra mật khẩu cũ
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, user.Password))
            {
                return BadRequest(new { Message = "Old password is incorrect" });
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu mới
            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
            {
                return BadRequest(new { Message = "New password and confirm password do not match" });
            }

            // Kiểm tra độ mạnh của mật khẩu mới
            if (!Regex.IsMatch(changePasswordDto.NewPassword, @"^(?=.*[A-Z]).{8,}$"))
            {
                return BadRequest(new { Message = "New password must have at least 8 characters and 1 uppercase letter" });
            }

            // Cập nhật mật khẩu mới
            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            // Sử dụng phương thức UpdateUserAsync của repository cũ
            await _userRepository.UpdateUserAsync(user);

            return Ok(new { Message = "Password changed successfully" });
        }

        [HttpPost("addPassword")]
        [Authorize] // Chỉ cho phép người dùng đã đăng nhập
        public async Task<IActionResult> AddPassword([FromBody] ChangePasswordDTO addPasswordDto)
        {
            // Lấy thông tin người dùng từ token JWT
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Unauthorized" });
            }

            // Lấy thông tin người dùng từ repository
            var user = await _userRepository.GetUserByIdAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu mới
            if (addPasswordDto.NewPassword != addPasswordDto.ConfirmNewPassword)
            {
                return BadRequest(new { Message = "New password and confirm password do not match" });
            }

            // Kiểm tra độ mạnh của mật khẩu mới
            if (!Regex.IsMatch(addPasswordDto.NewPassword, @"^(?=.*[A-Z]).{8,}$"))
            {
                return BadRequest(new { Message = "New password must have at least 8 characters and 1 uppercase letter" });
            }

            // Cập nhật mật khẩu mới
            user.Password = BCrypt.Net.BCrypt.HashPassword(addPasswordDto.NewPassword);

            // Sử dụng phương thức UpdateUserAsync của repository cũ
            await _userRepository.UpdateUserAsync(user);

            return Ok(new { Message = "Password changed successfully" });
        }

        // API: Track room usage expiration dates
        [HttpGet("{userId}/RoomUsageExpiration")]
        public async Task<IActionResult> GetRoomUsageExpirationDates(int userId)
        {
            try
            {
                // Lấy danh sách RentalLists từ repository
                var rentalLists = await _rentalListRepository.GetRentalListsAsync();

                // Lọc danh sách theo UserId
                var rentalIds = rentalLists
                    .Where(r => r.RenterID == userId)
                    .Select(r => new { r.RoomId, r.ContractId })
                    .ToList();

                if (!rentalIds.Any())
                {
                    return NotFound("Không có dữ liệu về ngày hết hạn sử dụng phòng.");
                }

                // Lấy danh sách Contracts từ repository
                var contracts = await _contractRepository.GetContractsAsync();

                // Lọc hợp đồng liên quan đến RentalLists
                var expirationDates = contracts
                    .Where(c => rentalIds.Any(r => r.ContractId == c.ContractId))
                    .Select(c => new
                    {
                        RoomId = rentalIds.First(r => r.ContractId == c.ContractId).RoomId,
                        ExpirationDate = c.RentalDateTimeEnd
                    })
                    .ToList();

                if (!expirationDates.Any())
                {
                    return NotFound("Không có hợp đồng nào được liên kết với phòng.");
                }

                return Ok(expirationDates);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi lấy thông tin ngày hết hạn sử dụng phòng.");
            }
        }
        [HttpGet("BankAccount")]
        [Authorize]// moi sua
        public async Task<ActionResult<IEnumerable<BankAccounts>>> GetUserBankAccounts()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                {
                    return BadRequest("UserId claim not found.");
                }
                int.TryParse(userIdClaim.Value, out int userId);
                var bankAccounts = await _userRepository.GetUserBankAccounts(userId);
                return Ok(bankAccounts);

            }
            catch (KeyNotFoundException ex)
            {
                return Ok(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching bank accounts.", details = ex.Message });
            }
        }

        //This function require an otp which is called from otp api
        [HttpPost("BankAccount")]
        [Authorize]
        public async Task<ActionResult<BankAccounts>> CreateUserBankAccount(BankAccountsDTO bankAccounts, string otp)
        {
            var verifiedOtp = _otpService.CheckOtp(otp) != null;
            if (!verifiedOtp)
            {
                return NotFound("Wrong otp");
            }
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }
                int.TryParse(userIdClaim.Value, out int userId);
                var userInfo = await _userRepository.GetUserByIdAsync(userId);
                if (userInfo == null)
                {
                    return NotFound("User not found.");
                }

                // Kiểm tra xem số tài khoản và mã ngân hàng đã tồn tại chưa
                bool accountExists = await _userRepository.CheckBankAccountExistsAsync(bankAccounts.AccountNumber, bankAccounts.BankCode);
                if (accountExists)
                {
                    return BadRequest("Bank account with the same account number and bank code already exists.");
                }

                var newBankAccount = await _userRepository.CreateNewBankAccounts(userId, bankAccounts);
                _otpService.RemoveOtp(userInfo.Gmail);
                return CreatedAtAction(nameof(GetUserBankAccounts), new { userId = userId }, newBankAccount);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the bank account.", details = ex.Message });
            }
        }


        //This function require an otp which is called from otp api
        [HttpPut("BankAccount")]
        public async Task<ActionResult<Boolean>> UpdateBankAccount([FromBody] BankAccountUpdateDto bankAccountUpdateDto)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }
                int.TryParse(userIdClaim.Value, out int userId);
                var userInfo = await _userRepository.GetUserByIdAsync(userId);
                if (userInfo == null)
                {
                    return NotFound("User not found.");
                }

                // Only verify OTP if activating the bank account
                if (bankAccountUpdateDto.Active)
                {
                    var verifiedOtp = _otpService.CheckOtp(bankAccountUpdateDto.Otp) != null;
                    if (!verifiedOtp)
                    {
                        return NotFound("Wrong OTP.");
                    }
                }

                await _userRepository.UpdateBankAccountStatus(userId, bankAccountUpdateDto.BankAccountId, bankAccountUpdateDto.Active);

                // Remove OTP after successful activation
                if (bankAccountUpdateDto.Active)
                {
                    _otpService.RemoveOtp(userInfo.Gmail);
                }

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the bank account status." });
            }
        }


        [HttpGet("otp")]
        [Authorize] //moi sua
        public Task<IActionResult> GetOtp()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Task.FromResult<IActionResult>(BadRequest(new { Message = "Unauthorized!" }));
            }
            int.TryParse(userIdClaim.Value, out int userId);
            var email = _userRepository.GetUserByIdAsync(userId).GetAwaiter().GetResult().Gmail;
            if (email == null)
            {
                return Task.FromResult<IActionResult>(StatusCode(500, new { Message = "Server Error." }));
            }
            var otp = _otpService.GenerateOtp(email);

            var emailContent = $@"
                <p>Chào {email},</p>
                <p>Chúng tôi thấy bạn đang gửi yêu cầu đối với ngân hàng của bạn trên tài khoản DUVAS.</p>
                <p><b>Không nên chia sẻ thông tin này với bất kỳ ai.</b></p>
                <p><b>Mã OTP để xác thực là: <span style='font-size: 18px; color: #ee1414;'>{otp}</span></b></p>
                <p>Nếu đây không phải yêu cầu xác thực của bạn, bạn có thể bỏ qua email này. Có thể một ai đó đã gõ nhầm địa chỉ email của bạn.</p>
                <p>Chúng tôi xin chân thành cảm ơn.</p>
                <p><b>DUVAS Team</b></p>";

            _emailService.SendEmail(email, "Xác thực email", emailContent);
            return Task.FromResult<IActionResult>(Ok(new { Message = "Please check your email to get an otp." }));
        }
    }
}