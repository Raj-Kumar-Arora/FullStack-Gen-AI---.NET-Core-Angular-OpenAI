using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class OpenAiResponse
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }    
    }
}
