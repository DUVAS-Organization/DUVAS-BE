// Controllers/FPTAIController.cs
using DTO;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/fptai")]
    public class FPTAIController : ControllerBase
    {
        private readonly FPTAIService _fptaiService;

        public FPTAIController(FPTAIService fptaiService)
        {
            _fptaiService = fptaiService ?? throw new ArgumentNullException(nameof(fptaiService));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] FileUploadDTO uploadDto)
        {
            try
            {
                if (uploadDto?.File == null || uploadDto.File.Length == 0)
                    return BadRequest(new { Message = "Image file is required" });

                var result = await _fptaiService.UploadImageAsync(uploadDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Server error: {ex.Message}" });
            }
        }
    }
}