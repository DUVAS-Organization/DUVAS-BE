using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class FPTAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiUrl;

    public FPTAIService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = configuration["FPTAI:ApiKey"];
        _apiUrl = configuration["FPTAI:ApiUrl"];
    }

    public async Task<ExtractedDataDTO> UploadImageAsync(FileUploadDTO file)
    {
        if (file?.File == null)
            throw new ArgumentNullException(nameof(file));

        using var stream = file.File.OpenReadStream();
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(stream), "image", file.FileName);

        _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);
        var response = await _httpClient.PostAsync(_apiUrl, content);
        var jsonString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ExtractedDataDTO>(jsonString);
    }


}