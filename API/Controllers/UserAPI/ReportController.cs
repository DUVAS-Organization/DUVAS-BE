using DTO;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.IRepository;
using API.Service; // Thêm namespace cho EmailService
using System.Linq; // Đảm bảo có using này cho phương thức Any()

namespace API.Controllers.UserAPI
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly EmailService _emailService; // Thêm EmailService
        private readonly IUserRepository _userRepository; // Thêm để lấy thông tin chủ phòng

        // Sửa constructor để inject EmailService và IUserRepository
        public ReportController(
            IReportRepository reportRepository,
            EmailService emailService,
            IUserRepository userRepository)
        {
            _reportRepository = reportRepository;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> AddReport([FromBody] AddReportDto reportDto)
        {
            // Kiểm tra xem user đã có report chưa xử lý cho phòng này chưa
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return BadRequest("UserId claim not found.");
            }
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid UserId.");
            }

            // Kiểm tra xem đã có report nào chưa xử lý (status = 0) cho phòng này chưa
            var existingReports = await _reportRepository.GetPendingReportsByUserAndRoomAsync(userId, reportDto.RoomId);
            if (existingReports.Any())
            {
                return BadRequest("Bạn đã có báo cáo chưa xử lý cho phòng này. Vui lòng chờ quản trị viên xử lý trước khi gửi báo cáo mới.");
            }

            Report report = new Report();
            report.RoomId = reportDto.RoomId;
            report.ReportContent = reportDto.ReportContent;
            report.Image = reportDto.Image;
            report.Status = 0;
            report.CreatedTime = DateTime.Now;
            report.UserId = userId;

            await _reportRepository.SaveReportAsync(report);

            // Gửi email thông báo cho chủ phòng
            try
            {
                // Lấy thông tin phòng và chủ phòng (giả sử có phương thức GetRoomOwnerIdAsync trong repository)
                var landlordId = await _reportRepository.GetRoomOwnerIdAsync(reportDto.RoomId);
                if (landlordId > 0)
                {
                    var landlord = await _userRepository.GetUserByIdAsync(landlordId);
                    if (landlord != null && !string.IsNullOrEmpty(landlord.Gmail))
                    {
                        // Tạo nội dung email
                        var subject = "Thông báo báo cáo mới về phòng của bạn";
                        var body = $@"
                            <p>Chào {landlord.Name},</p>
                            <p>Phòng của bạn vừa nhận được một báo cáo mới từ người dùng.</p>
                            <p><b>Nội dung báo cáo:</b> {reportDto.ReportContent}</p>
                            <p>Vui lòng kiểm tra và liên hệ với người thuê hoặc quản trị viên để giải quyết vấn đề.</p>
                            <p>Trân trọng,</p>
                            <p>DUVAS Team</p>";

                        // Gửi email
                        _emailService.SendEmail(landlord.Gmail, subject, body);
                    }
                }
            }
            catch (Exception ex)
            {
                // Không làm gì nếu gửi email thất bại, vì chức năng chính là tạo report
                Console.WriteLine($"Error sending email to landlord: {ex.Message}");
            }

            return Ok(new { message = "Report created successfully." });
        }

        // Các phương thức khác giữ nguyên...
        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                // Lấy danh sách báo cáo từ repository
                var reports = await _reportRepository.GetReportsAsync();

                // Ánh xạ từ Report sang ReportDTO
                var reportDtos = reports.Select(report => new ReportDTO
                {
                    ReportId = report.ReportId,
                    UserId = report.UserId,
                    RoomId = report.RoomId,
                    ReportContent = report.ReportContent,
                    Image = report.Image,
                    Status = report.Status,
                    Feedback = report.Feedback,
                    CreatedTime = report.CreatedTime
                }).ToList();

                return Ok(reportDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving reports.", error = ex.Message });
            }
        }

        [HttpGet("my-reports")]
        [Authorize]
        public async Task<IActionResult> GetMyReports()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                {
                    return BadRequest("UserId claim not found.");
                }
                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    return BadRequest("Invalid UserId.");
                }

                var reports = await _reportRepository.GetReportsByUserIdAsync(userId);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving your reports.", error = ex.Message });
            }
        }

        [HttpGet("has-pending-report/{roomId}")]
        [Authorize]
        public async Task<IActionResult> HasPendingReport(int roomId)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return BadRequest("Invalid UserId.");
                }

                bool hasPendingReport = await _reportRepository.HasPendingReport(userId, roomId);
                return Ok(new { hasPendingReport });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
    }
}