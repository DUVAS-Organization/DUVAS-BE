using DTO;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.IRepository;

namespace API.Controllers.UserAPI
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> AddReport([FromBody] AddReportDto reportDto)
        {
            Report report = new Report();
            report.RoomId = reportDto.RoomId;
            report.ReportContent = reportDto.ReportContent;
            report.Image = reportDto.Image;
            report.Status = 0;
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return BadRequest("UserId claim not found.");
            }
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid UserId.");
            }
            report.UserId = userId;
            await _reportRepository.SaveReportAsync(report);
            return Ok(new { message = "Report created successfully." });
        }

        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                var reports = await _reportRepository.GetReportsAsync();

                var reportDtos = reports.Select(report => new ReportDTO
                {
                    ReportId = report.ReportId,
                    UserId = report.UserId,
                    RoomId = report.RoomId,
                    ReportContent = report.ReportContent,
                    Image = report.Image,
                    Status = report.Status,
                    Feedback = report.Feedback
                }).ToList();

                return Ok(reportDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving reports.", error = ex.Message });
            }
        }
    }
}