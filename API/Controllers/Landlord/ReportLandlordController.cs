using DataAccess;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System;
using System.Threading.Tasks;
using API.Service;
using DUVAS;

namespace API.Controllers.Landlord
{
    [Route("api/landlord/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportLandlordController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportLandlordController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("my-room-reports")]
        public async Task<IActionResult> GetReportsForMyRooms()
        {
            try
            {

                int landlordId = GetLandlordId();

                if (!await IsLandlord(landlordId))
                {
                    return Unauthorized("Bạn không có quyền truy cập chức năng này.");
                }

                var reports = await _reportRepository.GetReportsByLandlordIdAsync(landlordId);

                if (reports == null || reports.Count == 0)
                {
                    return NotFound("Hiện không có báo cáo nào cho phòng của bạn.");
                }

                return Ok(new { message = "Danh sách báo cáo cho phòng của bạn.", reports });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetReportsForMyRooms: {ex.Message}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy báo cáo.", error = ex.Message });
            }
        }

        // Helper method to extract landlordId from JWT token
        private int GetLandlordId()
        {
            var userIdClaim = User.FindFirst("UserId"); // Lấy claim "UserId" thay vì NameIdentifier
            var landlordId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
            Console.WriteLine($"LandlordId: {landlordId}");
            return landlordId;
        }

        private async Task<bool> IsLandlord(int userId)
        {
            var user = await UserDAO.FindUserByIdAsync(userId);
            Console.WriteLine($"UserId: {userId}, RoleLandlord: {user?.RoleLandlord}");
            return user?.RoleLandlord == 1;
        }
    }
}