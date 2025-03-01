using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DUVAS;
using Repositories.IRepository;
using Repositories;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomManagementController : ControllerBase
    {
        private readonly IRentalListRepository _rentalListRepository;
        private readonly IUserFeedbackRepository _userFeedbackRepository;
        private readonly IRoomRepository _roomRepository;

        public RoomManagementController(IRentalListRepository rentalListRepository, IUserFeedbackRepository userFeedbackRepository, IRoomRepository roomRepository)
        {
            _rentalListRepository = rentalListRepository;
            _userFeedbackRepository = userFeedbackRepository;
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// API cho thuê phòng
        /// </summary>
        [HttpPost("rent-room")]
        public async Task<IActionResult> RentRoom([FromBody] RentalList rentalRequest)
        {
            if (rentalRequest == null || rentalRequest.RoomId <= 0 || rentalRequest.RenterID <= 0)
            {
                return BadRequest("Yêu cầu thuê không hợp lệ.");
            }

            const int DAILY_RENTAL_LIMIT = 1; // Số lượt thuê tối đa mỗi ngày (có thể điều chỉnh)
            DateTime today = DateTime.Today;

            try
            {
                // Lấy danh sách các yêu cầu thuê của người dùng trong ngày hôm nay có trạng thái 'mới' (0)
                var userRentals = await _rentalListRepository.GetRentalsByUserIdAsync(rentalRequest.RenterID);
                bool hasActiveRentalToday = userRentals.Any(r => r.CreatedDate.Date == today && r.RentalStatus == 0);

                if (hasActiveRentalToday)
                {
                    return BadRequest("Bạn đã đạt giới hạn thuê phòng trong ngày.");
                }

                // Ghi nhận yêu cầu thuê (không thay đổi trạng thái của phòng)
                rentalRequest.RentalStatus = 0; // Yêu cầu thuê mới
                rentalRequest.CreatedDate = DateTime.Now; // Ghi nhận thời điểm tạo
                await _rentalListRepository.SaveRentalListAsync(rentalRequest);

                // Gọi API gửi thông báo đến chủ phòng
                // var room = await _roomRepository.GetRoomByIdAsync(rentalRequest.RoomId);
                // if (room != null)
                // {
                //     // Gọi API Gửi mail ở đây
                // }

                return Ok("Yêu cầu thuê phòng đã được tạo thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }


        /// <summary>
        /// API hủy thuê phòng
        /// </summary>
        [HttpPut("cancel-room/{rentalId}")]
        public async Task<IActionResult> CancelRoom(int rentalId)
        {
            var rental = await _rentalListRepository.GetRentalListByIdAsync(rentalId);
            if (rental == null)
            {
                return NotFound("Không tìm thấy yêu cầu thuê.");
            }

            // Cập nhật trạng thái thuê thành 'Hủy' (giá trị -1)
            rental.RentalStatus = -1;
            await _rentalListRepository.UpdateRentalListAsync(rental);

            // Sau khi hủy, yêu cầu thuê không còn tính là 'mới' nên số lượt thuê trong ngày của người dùng được phục hồi tự động
            // (do trong truy vấn kiểm tra, chỉ tính các yêu cầu có RentalStatus == 0)

            // Gọi API gửi thông báo đến chủ phòng về việc hủy thuê
            var room = await _roomRepository.GetRoomByIdAsync(rental.RoomId);
            if (room != null)
            {
                //Gọi API Gửi mail ở đây
            }

            return Ok("Yêu cầu thuê phòng đã được hủy thành công.");
        }



        /// <summary>
        /// View room rental status
        /// </summary>
        [HttpGet("rental-status/{roomId}")]
        public async Task<IActionResult> ViewRoomRentalStatus(int roomId)
        {
            var rentals = await _rentalListRepository.GetRentalListsAsync();
            var roomRentals = rentals.FindAll(r => r.RoomId == roomId);

            if (roomRentals.Count == 0)
            {
                return NotFound("No rentals found for the specified room.");
            }

            return Ok(roomRentals);
        }


        /// <summary>
        /// View room reviews
        /// </summary>
        [HttpGet("room-reviews/{roomId}")]
        public async Task<IActionResult> ViewRoomReviews(int roomId)
        {
            var feedbacks = await _userFeedbackRepository.GetUserFeedbacksAsync();
            var roomFeedbacks = feedbacks.FindAll(f => f.UserId == roomId); // Assuming feedback is linked to the room via UserId

            if (roomFeedbacks.Count == 0)
            {
                return NotFound("No reviews found for the specified room.");
            }

            return Ok(roomFeedbacks);
        }

    }
}
