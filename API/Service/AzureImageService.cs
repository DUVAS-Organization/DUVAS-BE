
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace API.Service
{
    public class AzureImageService
    {
        private readonly ComputerVisionClient _client;

        public AzureImageService(string endpoint, string apiKey)
        {
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(apiKey))
            {
                Endpoint = endpoint
            };
        }

        public async Task<string> CheckImageAsync(Stream imageStream)
        {
            var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Adult };

            var analysis = await _client.AnalyzeImageInStreamAsync(imageStream, features);

            if (analysis.Adult.IsAdultContent || analysis.Adult.IsRacyContent || analysis.Adult.IsGoryContent)
            {
                return $"Ảnh phản cảm: \n" +
                       $"- Adult: {analysis.Adult.AdultScore}\n" +
                       $"- Racy: {analysis.Adult.RacyScore}\n" +
                       $"- Gore: {analysis.Adult.GoreScore}";
            }

            return "Ảnh an toàn.";
        }
    }
}