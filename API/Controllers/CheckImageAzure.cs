using API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckImageAzure : ControllerBase
    {
        private readonly AzureImageService _azureImageService;

        public CheckImageAzure(AzureImageService azureImageService)
        {
            _azureImageService = azureImageService;
        }
        [HttpPost("check-image")]
        public async Task<IActionResult> CheckImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Vui lòng chọn một ảnh để kiểm tra.");

            using var stream = file.OpenReadStream();
            var result = await _azureImageService.CheckImageAsync(stream);
            return Ok(result);
        }
    }
}