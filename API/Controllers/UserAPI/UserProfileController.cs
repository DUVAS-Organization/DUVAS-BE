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
        private readonly IRentalListRepository _rentalListRepository;
        private readonly IRentalServiceListRepository _rentalServiceListRepository;
        private readonly IContractRepository _contractRepository;

        public UserProfileController(IUserRepository userRepository, IRoomRepository roomRepository, 
            IServicePostRepository servicePostRepository, IRentalListRepository rentalListRepository, IContractRepository contractRepository,
            IRentalServiceListRepository rentalServiceListRepository)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _servicePostRepository = servicePostRepository;
            _rentalListRepository = rentalListRepository;
            _contractRepository = contractRepository;
            _rentalServiceListRepository = rentalServiceListRepository;
        }

        [HttpPut("{id}/ChangePassword")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordModel model)
        {
            if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest("Mật khẩu không hợp lệ.");
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User không tồn tại.");
            }

            if (user.Password != model.OldPassword)
            {
                return BadRequest("Mật khẩu cũ không đúng.");
            }

            user.Password = model.NewPassword;
            await _userRepository.UpdateUserAsync(user);

            return NoContent();
        }

        public class ChangePasswordModel
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        [HttpPut("{id}/EditProfile")]
        public async Task<IActionResult> EditProfile(int id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.UserId)
            {
                return BadRequest("ID không khớp.");
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User không tồn tại.");
            }

            user.UserName = updatedUser.UserName ?? user.UserName;
            user.Name = updatedUser.Name ?? user.Name;
            //user.Gmail = updatedUser.Gmail ?? user.Gmail;
            user.Phone = updatedUser.Phone ?? user.Phone;
            user.Address = updatedUser.Address ?? user.Address;
            user.Sex = updatedUser.Sex ?? user.Sex;
            user.ProfilePicture = updatedUser.ProfilePicture ?? user.ProfilePicture;

            await _userRepository.UpdateUserAsync(user);

            return NoContent();
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

    }
}
