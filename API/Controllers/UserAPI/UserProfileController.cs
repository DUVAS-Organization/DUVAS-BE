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
using Repositories.IRepository; // Thay bằng namespace chứa các interface repository

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IServicePostRepository _servicePostRepository;

        public UserProfileController(IUserRepository userRepository, IRoomRepository roomRepository, IServicePostRepository servicePostRepository)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _servicePostRepository = servicePostRepository;
        }

        // API: Edit profile
        [HttpPut("{id}/EditProfile")]
        public async Task<IActionResult> EditProfile(int id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.UserId)
            {
                return BadRequest("ID không khớp.");
            }

            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User không tồn tại.");
                }

                // Cập nhật thông tin
                user.UserName = updatedUser.UserName ?? user.UserName;
                user.Name = updatedUser.Name ?? user.Name;
                user.Gmail = updatedUser.Gmail ?? user.Gmail;
                user.Password = updatedUser.Password ?? user.Password;
                user.Phone = updatedUser.Phone ?? user.Phone;
                user.Address = updatedUser.Address ?? user.Address;
                user.Sex = updatedUser.Sex ?? user.Sex;
                user.ProfilePicture = updatedUser.ProfilePicture ?? user.ProfilePicture;

                await _userRepository.UpdateUserAsync(user);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi cập nhật thông tin.");
            }
        }

        // API: View room rental history
        [HttpGet("{id}/RoomRentalHistory")]
        public async Task<IActionResult> GetRoomRentalHistory(int id)
        {
            try
            {
                var history = await _roomRepository.GetRentalHistoryByUserIdAsync(id);
                if (history == null)
                {
                    return NotFound("Không có lịch sử thuê phòng.");
                }

                return Ok(history);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi lấy lịch sử thuê phòng.");
            }
        }

        // API: View service history
        [HttpGet("{id}/ServiceHistory")]
        public async Task<IActionResult> GetServiceHistory(int id)
        {
            try
            {
                var history = await _servicePostRepository.GetServiceHistoryByUserIdAsync(id);
                if (history == null)
                {
                    return NotFound("Không có lịch sử sử dụng dịch vụ.");
                }

                return Ok(history);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi lấy lịch sử sử dụng dịch vụ.");
            }
        }

        // API: Track room usage expiration dates
        [HttpGet("{id}/RoomUsageExpiration")]
        public async Task<IActionResult> GetRoomUsageExpirationDates(int id)
        {
            try
            {
                var expirationDates = await _roomRepository.GetRoomUsageExpirationByUserIdAsync(id);
                if (expirationDates == null)
                {
                    return NotFound("Không có dữ liệu về ngày hết hạn sử dụng phòng.");
                }

                return Ok(expirationDates);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi lấy thông tin ngày hết hạn sử dụng phòng.");
            }
        }

        // API: Track service usage expiration dates
        [HttpGet("{id}/ServiceUsageExpiration")]
        public async Task<IActionResult> GetServiceUsageExpirationDates(int id)
        {
            try
            {
                var expirationDates = await _servicePostRepository.GetServiceUsageExpirationByUserIdAsync(id);
                if (expirationDates == null)
                {
                    return NotFound("Không có dữ liệu về ngày hết hạn sử dụng dịch vụ.");
                }

                return Ok(expirationDates);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi lấy thông tin ngày hết hạn sử dụng dịch vụ.");
            }
        }
    }
}
