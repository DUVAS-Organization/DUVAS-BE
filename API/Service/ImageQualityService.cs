using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ImageQualityCheck
{
    public class AzureImageService
    {
        private readonly ComputerVisionClient _client;

        // Khởi tạo client với API Key và Endpoint của Azure
        public AzureImageService(string endpoint, string apiKey)
        {
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(apiKey))
            {
                Endpoint = endpoint
            };
        }

        // Kiểm tra chất lượng ảnh
        public async Task<string> CheckImageQualityAsync(Stream imageStream)
        {
            var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Color };

            // Phân tích ảnh
            var analysis = await _client.AnalyzeImageInStreamAsync(imageStream, features);

            // Kiểm tra độ sáng của ảnh (dựa vào thông tin màu sắc)
            var exposureResult = CheckExposure(analysis.Color);

            if (exposureResult != "Normal")
            {
                return exposureResult;
            }

            return "Ảnh có độ sáng bình thường.";
        }

        // Phương thức kiểm tra ảnh có sáng quá (overexposed) hay tối quá (underexposed)
        private string CheckExposure(ColorInfo colorInfo)
        {
            if (colorInfo.DominantColorBackground == "Black" || colorInfo.DominantColorForeground == "Black")
            {
                return "Ảnh có thể tối quá (underexposed).";
            }
            else if (colorInfo.DominantColorBackground == "White" || colorInfo.DominantColorForeground == "White")
            {
                return "Ảnh có thể sáng quá (overexposed).";
            }

            return "Normal"; // Độ sáng bình thường
        }
    }
}
