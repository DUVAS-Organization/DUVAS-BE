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
        public ReportAdminController(IReportRepository reportRepository, IRoomRepository roomRepository)
        {
            _reportRepository = reportRepository;
            _roomRepository = roomRepository;
        }

        [HttpPut("reject/{reportId}")]
        public async Task<IActionResult> RejectReport(int reportId)
        {
            try
            {
                // Tìm báo cáo
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

                int landlordId = await _reportRepository.GetRoomOwnerIdAsync(roomId.Value);
                if (landlordId == 0)
                {
                    return BadRequest("Không tìm thấy chủ phòng.");
                }

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
                await _reportRepository.LockRoomAsync(reportId);
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
                await _reportRepository.LockAccountAsync(reportId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi khóa tài khoản.", error = ex.Message });
            }
        }
    }
}