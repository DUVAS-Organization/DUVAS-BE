using API.Service;
using BusinessObject;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;

namespace API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportAdminController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IReportRepository _reportRepository;
        private readonly EmailService _emailService;
        private readonly IUserRepository _userRepository;
        public ReportAdminController(IReportRepository reportRepository, IRoomRepository roomRepository, EmailService emailService, IUserRepository userRepository)
        {
            _reportRepository = reportRepository;
            _roomRepository = roomRepository;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        [HttpPut("reject/{reportId}")]
        public async Task<IActionResult> RejectReport(int reportId)
        {
            try
            {
                var report = await _reportRepository.GetReportByIdAsync(reportId);
                if (report == null)
                {
                    return NotFound("Báo cáo không tồn tại.");
                }

                // Cập nhật trạng thái báo cáo thành 3 (Từ chối)
                report.Status = 3;
                await _reportRepository.UpdateReportAsync(report);

                int? roomId = report.RoomId;
                if (!roomId.HasValue)
                {
                    return BadRequest("Báo cáo không có RoomId liên kết.");
                }

                // Lấy thông tin phòng để gửi email
                var room = await _roomRepository.GetRoomByIdAsync(roomId.Value); 
                if (room == null)
                {
                    return BadRequest("Phòng không tồn tại.");
                }

                // Lấy thông tin người gửi báo cáo
                var reporter = await _userRepository.GetUserByIdAsync(report.UserId);
                if (reporter == null || string.IsNullOrEmpty(reporter.Gmail))
                {
                    return BadRequest("Không tìm thấy thông tin người gửi báo cáo hoặc email không hợp lệ.");
                }

                // Lấy thông tin chủ phòng
                int landlordId = await _reportRepository.GetRoomOwnerIdAsync(roomId.Value);
                if (landlordId == 0)
                {
                    return BadRequest("Không tìm thấy chủ phòng.");
                }
                var landlord = await _userRepository.GetUserByIdAsync(landlordId);
                if (landlord == null || string.IsNullOrEmpty(landlord.Gmail))
                {
                    return BadRequest("Không tìm thấy thông tin chủ phòng hoặc email không hợp lệ.");
                }

                // Nội dung email gửi cho người gửi báo cáo
                var reporterEmailContent = $@"
                    <p>Chào {reporter.Name},</p>
                    <p>Chúng tôi đã xem xét báo cáo của bạn đối với phòng tại {room.LocationDetail}.</p>
                    <p>Do bằng chứng không thể xác thực, báo cáo của bạn đã bị từ chối.</p>
                    <p>Nếu bạn có thêm thắc mắc, vui lòng liên hệ với chúng tôi.</p>
                    <p>Chúng tôi xin chân thành cảm ơn.</p>
                    <p><b>DUVAS Team</b></p>";

                // Gửi email cho người gửi báo cáo
                _emailService.SendEmail(reporter.Gmail, "Thông báo từ chối báo cáo", reporterEmailContent);

                // Nội dung email gửi cho chủ phòng
                var landlordEmailContent = $@"
                    <p>Chào {landlord.Name},</p>
                    <p>Báo cáo đối với phòng của bạn tại {room.LocationDetail} đã được xem xét.</p>
                    <p>Do không đủ bằng chứng, báo cáo này đã được gỡ bỏ.</p>
                    <p>Nếu bạn có thêm thắc mắc, vui lòng liên hệ với chúng tôi.</p>
                    <p>Chúng tôi xin chân thành cảm ơn.</p>
                    <p><b>DUVAS Team</b></p>";

                // Gửi email cho chủ phòng
                _emailService.SendEmail(landlord.Gmail, "Thông báo gỡ bỏ báo cáo", landlordEmailContent);

                // Gửi thông báo cho người dùng đã báo cáo
                var userNotification = new Notification
                {
                    UserId = report.UserId,
                    Type = "ReportRejected",
                    Message = "Bằng chứng không thể xác thực nên báo cáo của bạn đã bị hủy",
                    RedirectUrl = $"/Room/Details/{roomId}",
                    CreatedDate = DateTime.Now,
                    IsRead = false
                };
                await NotificationDAO.CreateNotificationAsync(userNotification);

                // Gửi thông báo cho chủ phòng
                var landlordNotification = new Notification
                {
                    UserId = landlordId,
                    Type = "ReportCancelled",
                    Message = "Báo cáo đối với phòng của bạn đã được hủy bỏ",
                    RedirectUrl = $"/Room/Details/{roomId}",
                    CreatedDate = DateTime.Now,
                    IsRead = false
                };
                await NotificationDAO.CreateNotificationAsync(landlordNotification);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi từ chối báo cáo.", error = ex.Message });
            }
        }

        [HttpPut("lock-room/{reportId}")]
        public async Task<IActionResult> LockRoom(int reportId)
        {
            try
            {
                var report = await _reportRepository.GetReportByIdAsync(reportId);
                if (report == null)
                {
                    return NotFound("Báo cáo không tồn tại.");
                }

                int? roomId = report.RoomId;
                if (!roomId.HasValue)
                {
                    return BadRequest("Báo cáo không có RoomId liên kết.");
                }

                // Lấy thông tin phòng
                var room = await _roomRepository.GetRoomByIdAsync(roomId.Value); 
                if (room == null)
                {
                    return BadRequest("Phòng không tồn tại.");
                }

                // Lấy thông tin người gửi báo cáo
                var reporter = await _userRepository.GetUserByIdAsync(report.UserId);
                if (reporter == null || string.IsNullOrEmpty(reporter.Gmail))
                {
                    return BadRequest("Không tìm thấy thông tin người gửi báo cáo hoặc email không hợp lệ.");
                }

                // Lấy thông tin chủ phòng
                int landlordId = await _reportRepository.GetRoomOwnerIdAsync(roomId.Value);
                if (landlordId == 0)
                {
                    return BadRequest("Không tìm thấy chủ phòng.");
                }
                var landlord = await _userRepository.GetUserByIdAsync(landlordId);
                if (landlord == null || string.IsNullOrEmpty(landlord.Gmail))
                {
                    return BadRequest("Không tìm thấy thông tin chủ phòng hoặc email không hợp lệ.");
                }

                // Khóa phòng
                await _reportRepository.LockRoomAsync(reportId);

                // Nội dung email gửi cho người gửi báo cáo
                var reporterEmailContent = $@"
                    <p>Chào {reporter.Name},</p>
                    <p>Chúng tôi đã xem xét báo cáo của bạn đối với phòng tại {room.LocationDetail}.</p>
                    <p>Báo cáo đã được xác nhận và phòng đã bị khóa bởi Admin.</p>
                    <p>Cảm ơn bạn đã giúp chúng tôi duy trì chất lượng dịch vụ.</p>
                    <p><b>DUVAS Team</b></p>";

                // Gửi email cho người gửi báo cáo
                _emailService.SendEmail(reporter.Gmail, "Thông báo khóa phòng", reporterEmailContent);

                // Nội dung email gửi cho chủ phòng
                var landlordEmailContent = $@"
                    <p>Chào {landlord.Name},</p>
                    <p>Phòng của bạn tại {room.LocationDetail} đã bị khóa bởi Admin do vi phạm chính sách.</p>
                    <p>Vui lòng liên hệ với chúng tôi để biết thêm chi tiết hoặc kháng nghị.</p>
                    <p><b>DUVAS Team</b></p>";

                // Gửi email cho chủ phòng
                _emailService.SendEmail(landlord.Gmail, "Thông báo khóa phòng", landlordEmailContent);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi khóa phòng.", error = ex.Message });
            }
        }

        [HttpPut("lock-account/{reportId}")]
        public async Task<IActionResult> LockAccount(int reportId)
        {
            try
            {
                var report = await _reportRepository.GetReportByIdAsync(reportId);
                if (report == null)
                {
                    return NotFound("Báo cáo không tồn tại.");
                }

                int? roomId = report.RoomId;
                if (!roomId.HasValue)
                {
                    return BadRequest("Báo cáo không có RoomId liên kết.");
                }

                // Lấy thông tin phòng
                var room = await _roomRepository.GetRoomByIdAsync(roomId.Value); 
                if (room == null)
                {
                    return BadRequest("Phòng không tồn tại.");
                }

                // Lấy thông tin người gửi báo cáo
                var reporter = await _userRepository.GetUserByIdAsync(report.UserId);
                if (reporter == null || string.IsNullOrEmpty(reporter.Gmail))
                {
                    return BadRequest("Không tìm thấy thông tin người gửi báo cáo hoặc email không hợp lệ.");
                }

                // Lấy thông tin chủ phòng 
                int landlordId = await _reportRepository.GetRoomOwnerIdAsync(roomId.Value);
                if (landlordId == 0)
                {
                    return BadRequest("Không tìm thấy chủ phòng.");
                }
                var landlord = await _userRepository.GetUserByIdAsync(landlordId);
                if (landlord == null || string.IsNullOrEmpty(landlord.Gmail))
                {
                    return BadRequest("Không tìm thấy thông tin chủ phòng hoặc email không hợp lệ.");
                }

                await _reportRepository.LockAccountAsync(reportId);

                // Nội dung email gửi cho người gửi báo cáo
                var reporterEmailContent = $@"
                    <p>Chào {reporter.Name},</p>
                    <p>Chúng tôi đã xem xét báo cáo của bạn đối với phòng tại {room.LocationDetail}.</p>
                    <p>Do vi phạm nghiêm trọng, tài khoản của chủ phòng đã bị khóa.</p>
                    <p>Cảm ơn bạn đã giúp chúng tôi duy trì chất lượng dịch vụ.</p>
                    <p><b>DUVAS Team</b></p>";

                // Gửi email cho người gửi báo cáo
                _emailService.SendEmail(reporter.Gmail, "Thông báo khóa tài khoản", reporterEmailContent);

                // Nội dung email gửi cho chủ phòng
                var landlordEmailContent = $@"
                    <p>Chào {landlord.Name},</p>
                    <p>Tài khoản của bạn đã bị khóa do vi phạm nghiêm trọng chính sách liên quan đến phòng tại {room.LocationDetail}.</p>
                    <p>Vui lòng liên hệ với chúng tôi để biết thêm chi tiết hoặc kháng nghị.</p>
                    <p><b>DUVAS Team</b></p>";

                // Gửi email cho chủ phòng
                _emailService.SendEmail(landlord.Gmail, "Thông báo khóa tài khoản", landlordEmailContent);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi khóa tài khoản.", error = ex.Message });
            }
        }
    }
}

