using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API.Service
{
    public class AiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AiService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["MistralAI:ApiKey"]; // API Key của Mistral AI

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("API Key của Mistral không tồn tại trong cấu hình.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<(string title, string description)> GenerateRoomTitleAndDescription(string roomInfo)
        {
            string url = "https://api.mistral.ai/v1/chat/completions"; // URL chính xác của Mistral AI

            var requestBody = new
            {
                model = "mistral-small", // Chỉ định model bạn muốn sử dụng
                messages = new[]
                {
                    new { role = "system", content = "Bạn là một AI giúp tạo tiêu đề và mô tả phòng." },
                    new { role = "user", content = $"Hãy tạo tiêu đề và mô tả đầy đủ cho phòng dựa trên thông tin sau: {roomInfo}. Mô tả phải chi tiết và ít nhất 100 từ." }
                },
                max_tokens = 1000 // Tăng giá trị này để đảm bảo đủ token cho output
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            Console.WriteLine($"[LOG] JSON gửi đi: {jsonBody}"); // Kiểm tra log

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] Phản hồi lỗi từ API: {errorContent}");
                throw new Exception($"Không thể tạo tiêu đề và mô tả phòng từ AI. Mã lỗi: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[LOG] Phản hồi từ API: {responseContent}"); // Log phản hồi từ API

            var resultJson = JObject.Parse(responseContent);

            // Kiểm tra phản hồi API có hợp lệ không
            var choices = resultJson["choices"];
            if (choices == null || choices.Type == JTokenType.Null || !choices.Any())
            {
                throw new Exception("Phản hồi từ AI không hợp lệ.");
            }

            var generatedText = resultJson["choices"]?[0]?["message"]?["content"]?.ToString();

            if (string.IsNullOrEmpty(generatedText))
            {
                throw new Exception("Không nhận được dữ liệu hợp lệ từ AI.");
            }

            // Chia kết quả thành từng dòng để lấy tiêu đề và mô tả
            var lines = generatedText.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string title = lines.Length > 0 ? lines[0].Trim().Replace("\"", "") : "Tiêu đề chưa có";
            string description = lines.Length > 1 ? string.Join(" ", lines.Skip(1)).Trim().Replace("\"", "") : "Mô tả chưa có";

            return (title, description);
        }
    }
}