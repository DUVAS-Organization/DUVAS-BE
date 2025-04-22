using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Service;
using ImageQualityCheck;


namespace API.Controllers.UserAPI

{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageQualityController : ControllerBase
    {
        private readonly AzureImageService _azureImageService;

        public ImageQualityController(AzureImageService azureImageService)
        {
            _azureImageService = azureImageService;
        }

        [HttpPost("check-image-quality")]
        public async Task<IActionResult> CheckImageQuality(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please provide an image to analyze.");

            using var stream = file.OpenReadStream();
            var result = await _azureImageService.CheckImageQualityAsync(stream);
            return Ok(result);
        }
    }

}
