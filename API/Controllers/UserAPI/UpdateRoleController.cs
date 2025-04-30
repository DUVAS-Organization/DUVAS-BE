using BusinessObject;
using DataAccess;
using DTO;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System;
using System.Threading.Tasks;
using BusinessObject.Service;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateRoleController : ControllerBase
    {
        private readonly ILandlordLicenseRepository _landlordLicenseRepository;
        private readonly IServiceLicenseRepository _serviceLicenseRepository;
        private readonly IUserRepository _userRepository;
        private readonly EncryptionService _encryptionService;
        private readonly int _adminId;

        public UpdateRoleController(
            ILandlordLicenseRepository landlordLicenseRepository,
            IServiceLicenseRepository serviceLicenseRepository,
            IUserRepository userRepository,
            IConfiguration configuration,
            EncryptionService encryptionService)
        {
            _landlordLicenseRepository = landlordLicenseRepository;
            _serviceLicenseRepository = serviceLicenseRepository;
            _userRepository = userRepository;
            _adminId = configuration.GetValue<int>("AdminId");
            _encryptionService = encryptionService;
        }

        // LandlordLicense Endpoints

        [HttpPost("Create-LandlordLicence")]
        [Authorize]
        public async Task<IActionResult> SaveLandlordLicense([FromBody] CreateLandlordLicenseDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            if (string.IsNullOrEmpty(dto.CCCD) || dto.CCCD.Length > 12)
            {
                return BadRequest("CCCD không hợp lệ (tối đa 12 ký tự).");
            }

            if (await _landlordLicenseRepository.IsCCCDExistsAsync(dto.CCCD) ||
                await _serviceLicenseRepository.IsCCCDExistsAsync(dto.CCCD))
            {
                return Conflict("CCCD đã được sử dụng trong một giấy phép khác.");
            }

            var user = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            if (user.RoleLandlord == 1)
            {
                return Conflict("Người dùng đã có giấy phép chủ nhà được phê duyệt.");
            }

            var pendingLandlordLicense = await _landlordLicenseRepository.GetByUserIdAsync(dto.UserId);
            if (pendingLandlordLicense != null)
            {
                return Conflict("Người dùng đã có một giấy phép chủ nhà đang chờ xử lý.");
            }

            var landlordLicense = new LandlordLicense(_encryptionService)
            {
                UserId = dto.UserId,
                AnhCCCDMatTruoc = dto.AnhCCCDMatTruoc,
                AnhCCCDMatSau = dto.AnhCCCDMatSau,
                CCCD = dto.CCCD,
                Name = dto.Name,
                dateOfBirth = dto.dateOfBirth,
                Sex = dto.Sex,
                Address = dto.Address,
                GiayPhepKinhDoanh = dto.GiayPhepKinhDoanh
            };

            await _landlordLicenseRepository.SaveLandlordLicenseAsync(landlordLicense);

            var message = "Bạn đã gửi thành công yêu cầu đăng ký làm chủ nhà.";
            var redirectUrl = "";
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

            return CreatedAtAction(nameof(SaveLandlordLicense), new { id = landlordLicense.LandlordLicenseId }, new { Message = "Đăng ký thành công.", LandlordLicenseId = landlordLicense.LandlordLicenseId });
        }

        [HttpGet("LandlordLicenses")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLandlordLicenses()
        {
            try
            {
                var result = await _landlordLicenseRepository.GetLandlordLicensesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách giấy phép chủ nhà: {ex.Message}");
            }
        }

        [HttpGet("LandlordLicense/{id}")]
        [Authorize]
        public async Task<IActionResult> GetLandlordLicenseById(int id)
        {
            try
            {
                var landlordLicense = await _landlordLicenseRepository.GetLandlordLicenseByIdAsync(id);
                if (landlordLicense == null)
                {
                    return NotFound("Giấy phép chủ nhà không tồn tại.");
                }

                var userIdClaim = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                if (userIdClaim != landlordLicense.UserId && !User.IsInRole("Admin"))
                {
                    return Forbid("Bạn không có quyền xem giấy phép này.");
                }

                var dto = new LandlordLicenseDTO
                {
                    LandlordLicenseId = landlordLicense.LandlordLicenseId,
                    UserId = landlordLicense.UserId,
                    CCCD = landlordLicense.CCCD,
                    Name = landlordLicense.Name,
                    dateOfBirth = landlordLicense.dateOfBirth,
                    Sex = landlordLicense.Sex,
                    Address = landlordLicense.Address,
                    GiayPhepKinhDoanh = landlordLicense.GiayPhepKinhDoanh,
                    Status = landlordLicense.Status
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy giấy phép chủ nhà: {ex.Message}");
            }
        }

        [HttpGet("LandlordLicense/ByUser/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetLandlordLicenseByUserId(int userId)
        {
            try
            {
                var userIdClaim = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                if (userIdClaim != userId && !User.IsInRole("Admin"))
                {
                    return Forbid("Bạn không có quyền xem giấy phép này.");
                }

                var landlordLicense = await _landlordLicenseRepository.GetByUserIdAsync(userId);
                if (landlordLicense == null)
                {
                    return NotFound("Người dùng này chưa có giấy phép chủ nhà.");
                }

                var dto = new LandlordLicenseDTO
                {
                    LandlordLicenseId = landlordLicense.LandlordLicenseId,
                    UserId = landlordLicense.UserId,
                    CCCD = landlordLicense.CCCD,
                    Name = landlordLicense.Name,
                    dateOfBirth = landlordLicense.dateOfBirth,
                    Sex = landlordLicense.Sex,
                    Address = landlordLicense.Address,
                    GiayPhepKinhDoanh = landlordLicense.GiayPhepKinhDoanh,
                    Status = landlordLicense.Status
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy giấy phép chủ nhà: {ex.Message}");
            }
        }

        [HttpPut("LandlordLicense/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLandlordLicense(int id, [FromBody] LandlordLicenseDTO dto)
        {
            try
            {
                if (id != dto.LandlordLicenseId)
                {
                    return BadRequest("ID không khớp.");
                }

                var existingLicense = await _landlordLicenseRepository.GetLandlordLicenseByIdAsync(id);
                if (existingLicense == null)
                {
                    return NotFound("Giấy phép chủ nhà không tồn tại.");
                }

                existingLicense.CCCD = dto.CCCD;
                existingLicense.Name = dto.Name;
                existingLicense.dateOfBirth = dto.dateOfBirth;
                existingLicense.Sex = dto.Sex;
                existingLicense.Address = dto.Address;
                existingLicense.GiayPhepKinhDoanh = dto.GiayPhepKinhDoanh;
                existingLicense.Status = dto.Status;

                await _landlordLicenseRepository.UpdateLandlordLicenseAsync(existingLicense);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật giấy phép chủ nhà: {ex.Message}");
            }
        }

        [HttpDelete("LandlordLicense/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLandlordLicense(int id)
        {
            try
            {
                var existingLicense = await _landlordLicenseRepository.GetLandlordLicenseByIdAsync(id);
                if (existingLicense == null)
                {
                    return NotFound("Giấy phép chủ nhà không tồn tại.");
                }

                await _landlordLicenseRepository.DeleteLandlordLicenseAsync(existingLicense);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xóa giấy phép chủ nhà: {ex.Message}");
            }
        }

        // ServiceLicense Endpoints

        [HttpPost("Create-ServiceLicence")]
        [Authorize]
        public async Task<IActionResult> SaveServiceLicense([FromBody] CreateServiceLicenseDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            if (string.IsNullOrEmpty(dto.CCCD) || dto.CCCD.Length > 12)
            {
                return BadRequest("CCCD không hợp lệ (tối đa 12 ký tự).");
            }

            if (await _landlordLicenseRepository.IsCCCDExistsAsync(dto.CCCD) ||
                await _serviceLicenseRepository.IsCCCDExistsAsync(dto.CCCD))
            {
                return Conflict("CCCD đã được sử dụng trong một giấy phép khác.");
            }

            var user = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            if (user.RoleService == 1)
            {
                return Conflict("Người dùng đã có giấy phép dịch vụ được phê duyệt.");
            }

            var pendingServiceLicense = await _serviceLicenseRepository.GetByUserIdAsync(dto.UserId);
            if (pendingServiceLicense != null)
            {
                return Conflict("Người dùng đã có một giấy phép dịch vụ đang chờ xử lý.");
            }

            var serviceLicense = new ServiceLicense(_encryptionService)
            {
                UserId = dto.UserId,
                AnhCCCDMatTruoc = dto.AnhCCCDMatTruoc,
                AnhCCCDMatSau = dto.AnhCCCDMatSau,
                CCCD = dto.CCCD,
                Name = dto.Name,
                dateOfBirth = dto.dateOfBirth,
                Sex = dto.Sex,
                Address = dto.Address,
                GiayPhepKinhDoanh = dto.GiayPhepKinhDoanh,
                GiayPhepChuyenMon = dto.GiayPhepChuyenMon
            };

            await _serviceLicenseRepository.SaveServiceLicenseAsync(serviceLicense);

            var message = "Bạn đã gửi thành công yêu cầu đăng ký làm chủ dịch vụ.";
            var redirectUrl = "";
            await NotificationDAO.CreateNotificationAsync(new Notification
            {
                UserId = serviceLicense.UserId,
                Type = "ConfirmUpdateRole",
                Message = message,
                RedirectUrl = redirectUrl,
                CreatedDate = DateTime.Now,
                IsRead = false
            });

            await NotificationDAO.CreateNotificationAsync(new Notification
            {
                UserId = _adminId,
                Type = "UpdateRole",
                Message = $"Vừa có đơn đăng ký làm chủ dịch vụ từ User: #{serviceLicense.UserId}",
                RedirectUrl = redirectUrl,
                CreatedDate = DateTime.Now,
                IsRead = false
            });

            return CreatedAtAction(nameof(SaveServiceLicense), new { id = serviceLicense.ServiceLicenseId }, new { Message = "Đăng ký thành công.", ServiceLicenseId = serviceLicense.ServiceLicenseId });
        }

        [HttpGet("ServiceLicenses")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetServiceLicenses()
        {
            try
            {
                var result = await _serviceLicenseRepository.GetServiceLicensesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách giấy phép dịch vụ: {ex.Message}");
            }
        }

        [HttpGet("ServiceLicense/{id}")]
        [Authorize]
        public async Task<IActionResult> GetServiceLicenseById(int id)
        {
            try
            {
                var serviceLicense = await _serviceLicenseRepository.GetServiceLicenseByIdAsync(id);
                if (serviceLicense == null)
                {
                    return NotFound("Giấy phép dịch vụ không tồn tại.");
                }

                var userIdClaim = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                if (userIdClaim != serviceLicense.UserId && !User.IsInRole("Admin"))
                {
                    return Forbid("Bạn không có quyền xem giấy phép này.");
                }

                var dto = new ServiceLicenseDTO
                {
                    ServiceLicenseId = serviceLicense.ServiceLicenseId,
                    UserId = serviceLicense.UserId,
                    CCCD = serviceLicense.CCCD,
                    Name = serviceLicense.Name,
                    dateOfBirth = serviceLicense.dateOfBirth,
                    Sex = serviceLicense.Sex,
                    Address = serviceLicense.Address,
                    GiayPhepKinhDoanh = serviceLicense.GiayPhepKinhDoanh,
                    GiayPhepChuyenMon = serviceLicense.GiayPhepChuyenMon,
                    Status = serviceLicense.Status
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy giấy phép dịch vụ: {ex.Message}");
            }
        }

        [HttpGet("ServiceLicense/ByUser/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetServiceLicenseByUserId(int userId)
        {
            try
            {
                var userIdClaim = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                if (userIdClaim != userId && !User.IsInRole("Admin"))
                {
                    return Forbid("Bạn không có quyền xem giấy phép này.");
                }

                var serviceLicense = await _serviceLicenseRepository.GetByUserIdAsync(userId);
                if (serviceLicense == null)
                {
                    return NotFound("Người dùng này chưa có giấy phép dịch vụ.");
                }

                var dto = new ServiceLicenseDTO
                {
                    ServiceLicenseId = serviceLicense.ServiceLicenseId,
                    UserId = serviceLicense.UserId,
                    CCCD = serviceLicense.CCCD,
                    Name = serviceLicense.Name,
                    dateOfBirth = serviceLicense.dateOfBirth,
                    Sex = serviceLicense.Sex,
                    Address = serviceLicense.Address,
                    GiayPhepKinhDoanh = serviceLicense.GiayPhepKinhDoanh,
                    GiayPhepChuyenMon = serviceLicense.GiayPhepChuyenMon,
                    Status = serviceLicense.Status
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy giấy phép dịch vụ: {ex.Message}");
            }
        }

        [HttpPut("ServiceLicense/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateServiceLicense(int id, [FromBody] ServiceLicenseDTO dto)
        {
            try
            {
                if (id != dto.ServiceLicenseId)
                {
                    return BadRequest("ID không khớp.");
                }

                var existingLicense = await _serviceLicenseRepository.GetServiceLicenseByIdAsync(id);
                if (existingLicense == null)
                {
                    return NotFound("Giấy phép dịch vụ không tồn tại.");
                }

                existingLicense.CCCD = dto.CCCD;
                existingLicense.Name = dto.Name;
                existingLicense.dateOfBirth = dto.dateOfBirth;
                existingLicense.Sex = dto.Sex;
                existingLicense.Address = dto.Address;
                existingLicense.GiayPhepKinhDoanh = dto.GiayPhepKinhDoanh;
                existingLicense.GiayPhepChuyenMon = dto.GiayPhepChuyenMon;
                existingLicense.Status = dto.Status;

                await _serviceLicenseRepository.UpdateServiceLicenseAsync(existingLicense);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật giấy phép dịch vụ: {ex.Message}");
            }
        }

        [HttpDelete("ServiceLicense/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteServiceLicense(int id)
        {
            try
            {
                var existingLicense = await _serviceLicenseRepository.GetServiceLicenseByIdAsync(id);
                if (existingLicense == null)
                {
                    return NotFound("Giấy phép dịch vụ không tồn tại.");
                }

                await _serviceLicenseRepository.DeleteServiceLicenseAsync(existingLicense);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xóa giấy phép dịch vụ: {ex.Message}");
            }
        }

        // Update Role Endpoints

        [HttpPut("UpdateRoleLandlord/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoleLandlord(int id, [FromBody] User updateRoleLandlord)
        {
            try
            {
                if (id != updateRoleLandlord.UserId)
                {
                    return BadRequest("ID không khớp.");
                }

                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("Người dùng không tồn tại.");
                }

                user.RoleLandlord = updateRoleLandlord.RoleLandlord;
                await _userRepository.UpdateUserAsync(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật vai trò chủ nhà: {ex.Message}");
            }
        }

        [HttpPut("UpdateRoleService/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoleService(int id, [FromBody] User updateRoleService)
        {
            try
            {
                if (id != updateRoleService.UserId)
                {
                    return BadRequest("ID không khớp.");
                }

                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("Người dùng không tồn tại.");
                }

                user.RoleService = updateRoleService.RoleService;
                await _userRepository.UpdateUserAsync(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật vai trò dịch vụ: {ex.Message}");
            }
        }
    }
}