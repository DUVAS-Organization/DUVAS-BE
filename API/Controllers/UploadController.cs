using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Service;
using Microsoft.Extensions.Logging;
using DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly ILogger<UploadController> _logger;

        public UploadController(CloudinaryService cloudinaryService, ILogger<UploadController> logger)
        {
            _cloudinaryService = cloudinaryService ?? throw new ArgumentNullException(nameof(cloudinaryService));
            _logger = logger;
        }

        [HttpPost("upload-image")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> UploadImage([FromForm] FileUploadDTO uploadDto)
        {
            var file = uploadDto.File;

            // Kiểm tra nếu không có file
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Kiểm tra loại file (chỉ chấp nhận image/*)
            if (!file.ContentType.StartsWith("image/"))
            {
                return BadRequest("Invalid file type. Please upload an image.");
            }

            // Giới hạn kích thước file (tối đa 5MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5MB
            if (file.Length > maxFileSize)
            {
                return BadRequest("File size exceeds the 5MB limit.");
            }

            try
            {
                using var stream = file.OpenReadStream();

                // Tên file mặc định nếu không có tên
                var fileName = string.IsNullOrWhiteSpace(file.FileName) ? "uploaded_image" : file.FileName;

                // Upload ảnh lên Cloudinary
                var imageUrl = await _cloudinaryService.UploadImageAsync(stream, fileName);
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during image upload.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while uploading the image.");
            }
        }



    }
}
