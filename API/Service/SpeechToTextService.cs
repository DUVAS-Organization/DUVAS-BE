using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using DTO;
using Newtonsoft.Json;

namespace API.Service
{
    public class SpeechToTextService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public SpeechToTextService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(bool Success, object Result, string ErrorMessage)> ConvertSpeechToTextAsync(FileUploadDTO audioFile)
        {
            var file = audioFile.File;
            if (file == null || file.Length == 0)
            {
                return (false, null, "No file uploaded");
            }

            var allowedTypes = new[] { "audio/wav", "audio/mpeg", "audio/mp3", "audio/ogg", "audio/webm" };
            if (!allowedTypes.Contains(file.ContentType))
            {
                return (false, null, "Unsupported file type.");
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
                    // Create HttpClient using IHttpClientFactory
                    var httpClient = _httpClientFactory.CreateClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                    request.Headers.Add("username", ""); // You can put the username here if needed
                    request.Headers.Add("api_key", apiKey); // Use the API key from appsettings.json
                    request.Content = new StreamContent(stream);

                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    // Deserialize the response into the DTO
                    var speechToTextResponse = JsonConvert.DeserializeObject<SpeechToTextResponse>(result);

                    // Return the first hypothesis
                    return (true, speechToTextResponse.Hypotheses[0], null);
                }
            }
            catch (Exception ex)
            {
                return (false, null, $"Internal server error: {ex.Message}");
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