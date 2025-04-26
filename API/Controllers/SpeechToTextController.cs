using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using DTO;
using Newtonsoft.Json;
using Repositories.IRepository;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechToTextController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IRoomRepository _roomRepository;

        public SpeechToTextController(IConfiguration configuration, IRoomRepository roomRepository)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _roomRepository = roomRepository;
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
                using (var stream = file.OpenReadStream())
                {
                    string apiKey = _configuration["FPTAI:ApiKey"];
                    string apiUrl = _configuration["FPTAI:ApiUrl"];

                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                    request.Headers.Add("api_key", apiKey);
                    request.Content = new StreamContent(stream);

                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    var speechToTextResponse = JsonConvert.DeserializeObject<SpeechToTextResponse>(result);
                    var hypothesisText = speechToTextResponse?.Hypotheses?.FirstOrDefault()?.Utterance;

                    if (string.IsNullOrWhiteSpace(hypothesisText))
                        return Ok(new { text = "Không nhận diện được âm thanh.", rooms = new List<RoomDTO>() });

                    var rooms = await _roomRepository.SearchRoomsByTermAsync(hypothesisText);

                    return Ok(new
                    {
                        text = hypothesisText,
                        rooms = rooms
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
