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
    public class ReportController:ControllerBase
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
            return Ok(new { message = "Withdraw request created successfully." });
        }
    }
}