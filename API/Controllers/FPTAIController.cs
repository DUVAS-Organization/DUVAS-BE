using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

[ApiController]
[Route("api/fptai")]
public class FPTAIController : ControllerBase
{
    private readonly FPTAIService _fptaiService;

    public FPTAIController(FPTAIService fptaiService)
    {
        _fptaiService = fptaiService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] FileUploadDTO uploadDto)
    {
        var file = uploadDto.File;
        if (file == null || file.Length == 0)
            return BadRequest("File is required");

        var result = await _fptaiService.UploadImageAsync(uploadDto);
        return Ok(result);
    }
}