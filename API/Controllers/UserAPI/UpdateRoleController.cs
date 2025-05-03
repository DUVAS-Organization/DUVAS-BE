using API.Service;
using BusinessObject;
using DataAccess;
using DTO;
using DUVAS;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using Repositories.IRepository;
using System.Threading.Tasks;
using API.Utils; // Sử dụng EncryptionHelper từ API.Utils

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateRoleController : ControllerBase
    {
        private readonly ILandlordLicenseRepository _landlordLicenseRepository;
        private readonly IServiceLicenseRepository _serviceLicenseRepository;
        private readonly IUserRepository _userRepository;
        private readonly int _adminId;

        public UpdateRoleController(ILandlordLicenseRepository landlordLicenseRepository,
            IUserRepository userRepository,
            IServiceLicenseRepository serviceLicenseRepository,
            IConfiguration configuration)
        {
            _landlordLicenseRepository = landlordLicenseRepository;
            _userRepository = userRepository;
            _serviceLicenseRepository = serviceLicenseRepository;
            _adminId = configuration.GetValue<int>("AdminId");
        }

        [HttpPost("Create-LandlordLicence")]
        public async Task<IActionResult> SaveLandlordLicense([FromBody] LandlordLicenseDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }
            // Validate CCCD format (e.g., max length 12 as per model)
            if (string.IsNullOrEmpty(dto.CCCD) || dto.CCCD.Length > 12)
            {
                return BadRequest("Invalid CCCD.");
            }
            Console.WriteLine($"Checking CCCD: {dto.CCCD}");
            // Check if CCCD already exists in LandlordLicense or ServiceLicense
            var existingLandlordLicense = await _landlordLicenseRepository.IsCCCDExistsAsync(dto.CCCD);
            Console.WriteLine($"CCCD exists in ServiceLicenses: {existingLandlordLicense}");
            if (existingLandlordLicense != false)
            {
                return Conflict("A license with this CCCD already exists.");
            }

            var user = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if user already has an approved LandlordLicense
            if (user.RoleLandlord == 1)
            {
                return Conflict("User already has an approved Landlord License.");
            }

            // Check if user has a pending LandlordLicense
            var pendingLandlordLicense = await _landlordLicenseRepository.GetByUserIdAsync(dto.UserId);
            if (pendingLandlordLicense != null)
            {
                return Conflict("User already has a pending Landlord License.");
            }

            var landlordLicense = new LandlordLicense
            {
                UserId = dto.UserId,
                AnhCCCDMatTruoc = EncryptionHelper.Encrypt(dto.AnhCCCDMatTruoc), // Mã hóa
                AnhCCCDMatSau = EncryptionHelper.Encrypt(dto.AnhCCCDMatSau),     // Mã hóa
                CCCD = dto.CCCD,
                Name = dto.Name,
                dateOfBirth = dto.dateOfBirth,
                Sex = dto.Sex,
                Address = dto.Address,
                GiayPhepKinhDoanh = dto.GiayPhepKinhDoanh
            };

            await _landlordLicenseRepository.SaveLandlordLicenseAsync(landlordLicense);

            // Gửi thông báo uprole
            var message = $"Bạn đã gửi thành công yêu cầu đăng ký làm chủ nhà.";
            var redirectUrl = $"";
            await NotificationDAO.CreateNotificationAsync(new Notification
            {
                UserId = landlordLicense.UserId,
                Type = "UpdateRole",
                Message = message,
                RedirectUrl = redirectUrl,
                CreatedDate = DateTime.Now,
                IsRead = false
            });

            await NotificationDAO.CreateNotificationAsync(new Notification
            {
                UserId = _adminId,
                Type = "UpdateRole",
                Message = $"Vừa có đơn đăng ký làm chủ nhà từ User: #{landlordLicense.UserId}",
                RedirectUrl = redirectUrl,
                CreatedDate = DateTime.Now,
                IsRead = false
            });

            // Giải mã trước khi trả về client
            landlordLicense.AnhCCCDMatTruoc = EncryptionHelper.Decrypt(landlordLicense.AnhCCCDMatTruoc);
            landlordLicense.AnhCCCDMatSau = EncryptionHelper.Decrypt(landlordLicense.AnhCCCDMatSau);

            return CreatedAtAction(nameof(SaveLandlordLicense), new { id = landlordLicense.LandlordLicenseId }, landlordLicense);
        }

        [HttpPost("Create-ServiceLicence")]
        public async Task<IActionResult> SaveServiceLicense([FromBody] ServiceLicenseDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }
            // Validate CCCD format (e.g., max length 12 as per model)
            if (string.IsNullOrEmpty(dto.CCCD) || dto.CCCD.Length > 12)
            {
                return BadRequest("Invalid CCCD.");
            }

            // Check if CCCD already exists in LandlordLicense or ServiceLicense
            var existingServiceLicense = await _serviceLicenseRepository.IsCCCDExistsAsync(dto.CCCD);
            if (existingServiceLicense != false)
            {
                return Conflict("A license with this CCCD already exists.");
            }

            // Check if user exists and their RoleService status
            var user = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if user already has an approved ServiceLicense
            if (user.RoleService == 1)
            {
                return Conflict("User already has an approved Service License.");
            }

            // Check if user has a pending ServiceLicense
            var pendingServiceLicense = await _serviceLicenseRepository.GetByUserIdAsync(dto.UserId);
            if (pendingServiceLicense != null)
            {
                return Conflict("User already has a pending Service License.");
            }

            var serviceLicense = new ServiceLicense
            {
                UserId = dto.UserId,
                AnhCCCDMatTruoc = EncryptionHelper.Encrypt(dto.AnhCCCDMatTruoc), // Mã hóa
                AnhCCCDMatSau = EncryptionHelper.Encrypt(dto.AnhCCCDMatSau),     // Mã hóa
                CCCD = dto.CCCD,
                Name = dto.Name,
                dateOfBirth = dto.dateOfBirth,
                Sex = dto.Sex,
                Address = dto.Address,
                GiayPhepKinhDoanh = dto.GiayPhepKinhDoanh,
                GiayPhepChuyenMon = dto.GiayPhepChuyenMon
            };

            await _serviceLicenseRepository.SaveServiceLicenseAsync(serviceLicense);

            // Gửi thông báo uprole
            var message = $"Bạn đã gửi thành công yêu cầu đăng ký làm chủ dịch vụ.";
            var redirectUrl = $"/ViewUpRole";
            await NotificationDAO.CreateNotificationAsync(new Notification
            {
                UserId = serviceLicense.UserId,
                Type = "ConfirmUpdateRole",
                Message = message,
                RedirectUrl = redirectUrl,
                CreatedDate = DateTime.Now,
                IsRead = false
            });

            // Giải mã trước khi trả về client
            serviceLicense.AnhCCCDMatTruoc = EncryptionHelper.Decrypt(serviceLicense.AnhCCCDMatTruoc);
            serviceLicense.AnhCCCDMatSau = EncryptionHelper.Decrypt(serviceLicense.AnhCCCDMatSau);

            return CreatedAtAction(nameof(SaveServiceLicense), new { id = serviceLicense.ServiceLicenseId }, serviceLicense);
        }

        [HttpPut("{id}/UpdateRoleLandlord")]
        public async Task<IActionResult> UpdateRoleLandlord(int id, [FromBody] User updateRoleLandlord)
        {
            if (id != updateRoleLandlord.UserId)
            {
                return BadRequest("ID không khớp.");
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User không tồn tại.");
            }

            user.RoleLandlord = 2;

            await _userRepository.UpdateUserAsync(user);

            return NoContent();
        }

        [HttpPut("{id}/UpdateRoleService")]
        public async Task<IActionResult> UpdateRoleService(int id, [FromBody] User updateRoleService)
        {
            if (id != updateRoleService.UserId)
            {
                return BadRequest("ID không khớp.");
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User không tồn tại.");
            }

            user.RoleService = 2; // Sửa lỗi: cập nhật RoleService thay vì RoleLandlord

            await _userRepository.UpdateUserAsync(user);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetLandlordLicenses()
        {
            var result = await _landlordLicenseRepository.GetLandlordLicensesAsync();
            foreach (var license in result)
            {
                license.AnhCCCDMatTruoc = EncryptionHelper.Decrypt(license.AnhCCCDMatTruoc);
                license.AnhCCCDMatSau = EncryptionHelper.Decrypt(license.AnhCCCDMatSau);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLandlordLicenseById(int id)
        {
            var result = await _landlordLicenseRepository.GetLandlordLicenseByIdAsync(id);
            if (result == null)
                return NotFound("License not found.");

            result.AnhCCCDMatTruoc = EncryptionHelper.Decrypt(result.AnhCCCDMatTruoc);
            result.AnhCCCDMatSau = EncryptionHelper.Decrypt(result.AnhCCCDMatSau);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLandlordLicense([FromBody] LandlordLicense license)
        {
            if (license == null)
                return BadRequest("Invalid data.");

            license.AnhCCCDMatTruoc = EncryptionHelper.Encrypt(license.AnhCCCDMatTruoc);
            license.AnhCCCDMatSau = EncryptionHelper.Encrypt(license.AnhCCCDMatSau);

            await _landlordLicenseRepository.SaveLandlordLicenseAsync(license);

            license.AnhCCCDMatTruoc = EncryptionHelper.Decrypt(license.AnhCCCDMatTruoc);
            license.AnhCCCDMatSau = EncryptionHelper.Decrypt(license.AnhCCCDMatSau);

            return StatusCode(201, "License created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLandlordLicense(int id, [FromBody] LandlordLicense license)
        {
            if (id != license.LandlordLicenseId)
                return BadRequest("ID mismatch.");

            license.AnhCCCDMatTruoc = EncryptionHelper.Encrypt(license.AnhCCCDMatTruoc);
            license.AnhCCCDMatSau = EncryptionHelper.Encrypt(license.AnhCCCDMatSau);

            await _landlordLicenseRepository.UpdateLandlordLicenseAsync(license);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLandlordLicense(int id)
        {
            var existingLicense = await _landlordLicenseRepository.GetLandlordLicenseByIdAsync(id);
            if (existingLicense == null)
                return NotFound("License not found.");

            await _landlordLicenseRepository.DeleteLandlordLicenseAsync(existingLicense);
            return NoContent();
        }

        // Service License API
        [HttpGet("service")]
        public async Task<IActionResult> GetServiceLicenses()
        {
            var result = await _serviceLicenseRepository.GetServiceLicensesAsync();
            foreach (var license in result)
            {
                license.AnhCCCDMatTruoc = EncryptionHelper.Decrypt(license.AnhCCCDMatTruoc);
                license.AnhCCCDMatSau = EncryptionHelper.Decrypt(license.AnhCCCDMatSau);
            }
            return Ok(result);
        }

        [HttpGet("service/{id}")]
        public async Task<IActionResult> GetServiceLicenseById(int id)
        {
            var result = await _serviceLicenseRepository.GetServiceLicenseByIdAsync(id);
            if (result == null)
                return NotFound("Service License not found.");

            result.AnhCCCDMatTruoc = EncryptionHelper.Decrypt(result.AnhCCCDMatTruoc);
            result.AnhCCCDMatSau = EncryptionHelper.Decrypt(result.AnhCCCDMatSau);

            return Ok(result);
        }

        [HttpPost("service")]
        public async Task<IActionResult> CreateServiceLicense([FromBody] ServiceLicense license)
        {
            if (license == null)
                return BadRequest("Invalid data.");

            license.AnhCCCDMatTruoc = EncryptionHelper.Encrypt(license.AnhCCCDMatTruoc);
            license.AnhCCCDMatSau = EncryptionHelper.Encrypt(license.AnhCCCDMatSau);

            await _serviceLicenseRepository.SaveServiceLicenseAsync(license);

            license.AnhCCCDMatTruoc = EncryptionHelper.Decrypt(license.AnhCCCDMatTruoc);
            license.AnhCCCDMatSau = EncryptionHelper.Decrypt(license.AnhCCCDMatSau);

            return StatusCode(201, "Service License created successfully.");
        }

        [HttpPut("service/{id}")]
        public async Task<IActionResult> UpdateServiceLicense(int id, [FromBody] ServiceLicense license)
        {
            if (id != license.ServiceLicenseId)
                return BadRequest("ID mismatch.");

            license.AnhCCCDMatTruoc = EncryptionHelper.Encrypt(license.AnhCCCDMatTruoc);
            license.AnhCCCDMatSau = EncryptionHelper.Encrypt(license.AnhCCCDMatSau);

            await _serviceLicenseRepository.UpdateServiceLicenseAsync(license);

            return NoContent();
        }

        [HttpDelete("service/{id}")]
        public async Task<IActionResult> DeleteServiceLicense(int id)
        {
            var existingLicense = await _serviceLicenseRepository.GetServiceLicenseByIdAsync(id);
            if (existingLicense == null)
                return NotFound("Service License not found.");

            await _serviceLicenseRepository.DeleteServiceLicenseAsync(existingLicense);
            return NoContent();
        }
    }
}