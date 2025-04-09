// Services/FPTAIService.cs
using DTO;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Services
{
    public class FPTAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;

        public FPTAIService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = configuration["FPTAI:ApiKey"] ?? throw new ArgumentNullException("FPTAI:ApiKey is missing in configuration");
            _apiUrl = configuration["FPTAI:ApiUrl"] ?? throw new ArgumentNullException("FPTAI:ApiUrl is missing in configuration");
        }

        public async Task<ExtractedDataDTO> UploadImageAsync(FileUploadDTO file)
        {
            if (file?.File == null || file.File.Length == 0)
                throw new ArgumentException("Image file cannot be empty");

            try
            {
                // Prepare request
                using var content = new MultipartFormDataContent();
                using var stream = file.File.OpenReadStream();
                content.Add(new StreamContent(stream), "image", file.FileName);

                // Set headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

                // Send request to FPT.AI
                var response = await _httpClient.PostAsync(_apiUrl, content);
                response.EnsureSuccessStatusCode();

                // Parse response
                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var fptResponse = JsonSerializer.Deserialize<FPTAIResponseDTO>(jsonString, options);

                // Check for FPT.AI errors
                if (fptResponse?.ErrorCode != 0)
                    throw new Exception($"FPT.AI error: {fptResponse?.ErrorMessage ?? "Unknown error"}");

                // Get data from response
                var data = fptResponse.Data?.FirstOrDefault();
                if (data == null)
                    return new ExtractedDataDTO();

                // Map to ExtractedDataDTO
                return new ExtractedDataDTO
                {
                    AnhCCCDMatTruoc = data.FrontImage,
                    AnhCCCDMatSau = data.BackImage,
                    CCCD = data.Id,
                    Name = data.Name,
                    dateOfBirth = DateTime.TryParse(data.Dob, out var dob) ? dob : null,
                    Sex = data.Sex,
                    Address = data.Address
                };
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error calling FPT.AI API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error parsing FPT.AI response: {ex.Message}", ex);
            }
        }
    }
}