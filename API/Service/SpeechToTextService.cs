using System.Net.Http.Headers; // For MediaTypeHeaderValue
using System.Net.Http; // For HttpClient
using System.Threading.Tasks;
using Newtonsoft.Json;
using DTO;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace API.Service
{
    public class SpeechToTextService
    {
        private readonly IConfiguration _configuration;

        public SpeechToTextService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> ConvertSpeechToTextAsync(IFormFile audioFile)
        {
            var file = audioFile;
            if (file == null || file.Length == 0)
            {
                throw new Exception("No file uploaded");
            }

            var allowedTypes = new[] { "audio/wav", "audio/mpeg", "audio/mp3", "audio/ogg", "audio/webm" };
            if (!allowedTypes.Contains(file.ContentType))
            {
                throw new Exception("Unsupported file type.");
            }

            try
            {
                var content = new MultipartFormDataContent();
                var byteArray = await FileToByteArray(file);
                var fileContent = new ByteArrayContent(byteArray);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "file", file.FileName);

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

                    // Check if Hypotheses is not null and has at least one element
                    if (speechToTextResponse?.Hypotheses?.Any() == true)
                    {
                        // Assuming Hypothesis has a 'Text' property that contains the actual text
                        return speechToTextResponse.Hypotheses[0].Text;  // Return the 'Text' from the first hypothesis
                    }
                    else
                    {
                        throw new Exception("No speech-to-text hypotheses returned.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while converting speech to text: {ex.Message}");
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
