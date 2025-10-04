using System.Text.Json;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Services
{
    public class WhisperService : IWhisperService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WhisperService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];
        }
        public async Task<string?> TranscribeAudioAsync(string filePath)
        {
            using var multipart = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(filePath));
            fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("audio/mpeg");
            multipart.Add(fileContent, "file", System.IO.Path.GetFileName(filePath));
            multipart.Add(new StringContent("whisper-1"), "model");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/audio/transcriptions", multipart);
            response.EnsureSuccessStatusCode();

            //var responseContent = await response.Content.ReadAsStringAsync();
            // Assuming the response is a JSON object with a "text" field
            // In a real implementation, you would deserialize the JSON response to extract the transcribed text.
            // For simplicity, returning a placeholder string here.
            // You can use Newtonsoft.Json or System.Text.Json to deserialize the response.
            // Example: var result = JsonSerializer.Deserialize<TranscriptionResult>(responseContent);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OpenAiResponse>(json)?.Text;
        }
    }
}