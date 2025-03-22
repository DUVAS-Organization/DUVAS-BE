using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using DTO;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechToTextController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public SpeechToTextController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertSpeechToText([FromForm] FileUploadDTO audioFile)
        {
            var file = audioFile.File;
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            var allowedTypes = new[] { "audio/wav", "audio/mpeg", "audio/mp3", "audio/ogg", "audio/webm" };
            if (!allowedTypes.Contains(file.ContentType))
            {
                return BadRequest("Unsupported file type.");
            }

            try
            {
                var content = new MultipartFormDataContent();
                var byteArray = await FileToByteArray(file);
                var fileContent = new ByteArrayContent(byteArray);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "File", file.FileName);

                // Get API key and URL from appsettings.json
                string apiKey = _configuration["FPTAI:ApiKey"];
                string apiUrl = _configuration["FPTAI:ApiUrl"];

                using (var stream = file.OpenReadStream())
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                    request.Headers.Add("username", ""); // You can put the username here if needed
                    request.Headers.Add("api_key", apiKey); // Use the API key from appsettings.json
                    request.Content = new StreamContent(stream);

                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    // Deserialize the response into the DTO
                    var speechToTextResponse = JsonConvert.DeserializeObject<SpeechToTextResponse>(result);

                    // Return the first hypothesis
                    return Ok(speechToTextResponse.Hypotheses[0]);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<byte[]> FileToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
