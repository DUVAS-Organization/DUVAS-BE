using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DUVAS;
using Repositories.IRepository;
using Repositories;
using API.Service;
using DTO;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomManagementController : ControllerBase
    {
        private readonly IRentalListRepository _rentalListRepository;
        private readonly IUserFeedbackRepository _userFeedbackRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly EmailService _emailService;
        private readonly IUserRepository _userRepository;

        public RoomManagementController(EmailService emailService, IRentalListRepository rentalListRepository, IUserFeedbackRepository userFeedbackRepository, IRoomRepository roomRepository, IUserRepository userRepository)
        {
            _rentalListRepository = rentalListRepository;
            _userFeedbackRepository = userFeedbackRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _emailService = emailService;
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
                var userRentals = await _rentalListRepository.GetRentalsByUserIdAsync(rentalRequest.RenterID);// Noi vs thang hung
                bool hasActiveRentalToday = userRentals.Any(r => r.CreatedDate.Date == today && r.RentalStatus == 0);

                if (hasActiveRentalToday)
                {
                    return BadRequest("Bạn đã đạt giới hạn thuê phòng trong ngày.");
                }

                // Ghi nhận yêu cầu thuê (không thay đổi trạng thái của phòng)
                rentalRequest.RentalStatus = 1; // Yêu cầu thuê mới
                rentalRequest.CreatedDate = DateTime.Now; // Ghi nhận thời điểm tạo
                await _rentalListRepository.SaveRentalListAsync(rentalRequest);

                // Gọi API gửi thông báo đến chủ phòng
                var room = await _roomRepository.GetRoomByIdAsync(rentalRequest.RoomId);

                // Gọi service track-room

                return Ok("Yêu cầu thuê phòng đã được tạo thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }


        /// <summary>
        /// API cho thuê phòng
        /// </summary>
        [HttpGet("track-room/{roomId}")]
        [Authorize]
        public async Task<IActionResult> TrackRoomStatus(int roomId)
        {
            var room = await _roomRepository.GetRoomByIdAsync(roomId);
            if (room == null)
            {
                return NotFound("Không tìm thấy phòng.");
            }

            string statusMessage;
            if (room.status == 1) // Phòng trống
            {
                statusMessage = "Phòng này đang trống và sẵn sàng cho thuê.";
            }
            else if (room.status == 2 || room.status == 3)
            {
                statusMessage = "Phòng này đang được thuê hoặc đang làm hợp đồng.";
            }
            else
            {
                statusMessage = "Trạng thái phòng không hợp lệ.";
            }

            return Ok(new { RoomId = roomId, Status = room.status, Message = statusMessage });
        }



        /// <summary>
        /// API cho thuê phòng
        /// </summary>
        [HttpPost("send-mail")]
        [Authorize]
        public async Task<IActionResult> sendMail([FromBody] SendMailDTO sendMailDTO)
        {
            var landlord = await _userRepository.GetUserByIdAsync(sendMailDTO.UserIdLandlord);

            if (landlord == null)
            {
                return BadRequest("Thông tin người thuê hoặc chủ phòng không hợp lệ.");
            }

            var room = await _roomRepository.GetRoomByIdAsync(sendMailDTO.RoomId);

            if (room == null)
            {
                return BadRequest("Phòng không tồn tại");
            }

            if (room.UserId != sendMailDTO.UserIdLandlord)
            {
                return BadRequest("Phòng không hợp lệ");
            }

            if (string.IsNullOrEmpty(landlord.Gmail))
            {
                return BadRequest("Mail của landlord lỗi");
            }

            _emailService.SendRentalNotificationToLandlord(landlord.Gmail!, sendMailDTO.RoomId, sendMailDTO.RenterName);

            // **🔥 Cập nhật trạng thái phòng thành Pending (2)**
            room.status = 2;
            await _roomRepository.UpdateRoomAsync(room);


            return Ok("Gửi mail thành công.");
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
        [HttpGet("rooms")]
        public async Task<IActionResult> GetAvailableRooms()
        {
            try
            {
                var rooms = await _roomRepository.GetAllRoomsByStatusAsync(1); 
                if (rooms == null || !rooms.Any())
                {
                    return NotFound("Không có phòng trống nào hiện tại.");
                }
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

    }
}